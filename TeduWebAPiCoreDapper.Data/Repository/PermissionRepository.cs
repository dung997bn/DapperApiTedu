using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using TeduWebAPiCoreDapper.Data.Repository.Interfaces;
using TeduWebAPiCoreDapper.Data.ViewModels;

namespace TeduWebAPiCoreDapper.Data.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly string _connectionString;

        public PermissionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<IEnumerable<FunctionViewModel>> GetAllFunctionByPermissionAsync(Guid userId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    await conn.OpenAsync();

                var paramaters = new DynamicParameters();
                paramaters.Add("@userId", userId);

                var result = await conn.QueryAsync<FunctionViewModel>(" ", paramaters, null, null, CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<PermissionViewModel>> GetAllRolePermissionsAsync(Guid? role)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    await conn.OpenAsync();

                var paramaters = new DynamicParameters();
                paramaters.Add("@roleId", role);

                var result = await conn.QueryAsync<PermissionViewModel>("Get_Permission_ByRoleId", paramaters, null, null, CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<FunctionActionViewModel>> GetAllWithPermissionAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    await conn.OpenAsync();

                var result = await conn.QueryAsync<FunctionActionViewModel>("Get_Function_WithActions", null, null, null, System.Data.CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task SavePermissionsAsync(Guid role, List<PermissionViewModel> permissions)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var dt = new DataTable();
                dt.Columns.Add("RoleId", typeof(Guid));
                dt.Columns.Add("FunctionId", typeof(string));
                dt.Columns.Add("ActionId", typeof(string));
                foreach (var item in permissions)
                {
                    dt.Rows.Add(role, item.FunctionId, item.ActionId);
                }
                var paramaters = new DynamicParameters();
                paramaters.Add("@roleId", role);
                paramaters.Add("@permissions", dt.AsTableValuedParameter("dbo.Permission"));
                await conn.ExecuteAsync("Create_Permission", paramaters, null, null, CommandType.StoredProcedure);
            }
        }
    }
}
