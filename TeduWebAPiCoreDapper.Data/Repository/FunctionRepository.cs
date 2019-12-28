using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduWebAPiCoreDapper.Data.Models;
using TeduWebAPiCoreDapper.Data.Repository.Interfaces;
using TeduWebAPiCoreDapper.Untilities.Dtos;

namespace TeduWebAPiCoreDapper.Data.Repository
{
    public class FunctionRepository : IFunctionRepository
    {
        private readonly string _connectionString;
        public FunctionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }
        public async Task CreateAsync(Function function)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var param = new DynamicParameters();
                param.Add("@id", function.Id);
                param.Add("@name", function.Name);
                param.Add("@url", function.Url);
                param.Add("@parentId", function.ParentId);
                param.Add("@sortOrder", function.SortOrder);
                param.Add("@cssClass", function.CssClass);
                param.Add("@isActive", function.IsActive);
                var result = await conn.ExecuteAsync("Create_Function", param, null, null, CommandType.StoredProcedure);
            }
        }

        public async Task DeleteAsync(string id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var param = new DynamicParameters();
                param.Add("@id", id);
                await conn.ExecuteAsync("Delete_Function_ById", param, null, null, CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Function>> GetAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    await conn.OpenAsync();

                var paramaters = new DynamicParameters();
                var result = await conn.QueryAsync<Function>("Get_Function_All", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<Function> GetByIdAsync(string id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var param = new DynamicParameters();
                param.Add("@id", id);
                var result = await conn.QueryAsync<Function>("Get_Function_ById", param, null, null, CommandType.StoredProcedure);
                return result.SingleOrDefault();
            }
        }

        public async Task<PagedResult<Function>> GetPagingAsync(string keyword, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    await conn.OpenAsync();

                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var result = await conn.QueryAsync<Function>("Get_Function_AllPaging", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                int totalRow = paramaters.Get<int>("@totalRow");

                var pagedResult = new PagedResult<Function>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                return pagedResult;
            }
        }

        public async Task UpdateAsync(string id, Function function)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var param = new DynamicParameters();
                param.Add("@id", id);
                param.Add("@name", function.Name);
                param.Add("@url", function.Url);
                param.Add("@parentId", function.ParentId);
                param.Add("@sortOrder", function.SortOrder);
                param.Add("@cssClass", function.CssClass);
                param.Add("@isActive", function.IsActive);
                await conn.ExecuteAsync("Update_Function", param, null, null, CommandType.StoredProcedure);
            }
        }
    }
}
