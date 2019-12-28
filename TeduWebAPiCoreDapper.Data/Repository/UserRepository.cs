using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TeduWebAPiCoreDapper.Data.Models;
using TeduWebAPiCoreDapper.Data.Repository.Interfaces;
using TeduWebAPiCoreDapper.Untilities.Dtos;

namespace TeduWebAPiCoreDapper.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
            _userManager = userManager;
        }

        public async Task AssignToRolesAsync(Guid id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var normalizedName = roleName.ToUpper();
                var roleId = await connection.ExecuteScalarAsync<Guid?>($"SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = @{nameof(normalizedName)}", new { normalizedName });
                if (!roleId.HasValue)
                {
                    roleId = Guid.NewGuid();
                    await connection.ExecuteAsync($"INSERT INTO [AspNetRoles]([Id],[Name], [NormalizedName]) VALUES(@{nameof(roleId)},@{nameof(roleName)}, @{nameof(normalizedName)})",
                       new { roleName, normalizedName });
                }


                await connection.ExecuteAsync($"IF NOT EXISTS(SELECT 1 FROM [AspNetUserRoles] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}) " +
                    $"INSERT INTO [AspNetUserRoles]([UserId], [RoleId]) VALUES(@userId, @{nameof(roleId)})",
                    new { userId = user.Id, roleId });
            }
        }

        public async Task<IdentityResult> CreateAsync(AppUser user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(string id)
        {
            var role = await _userManager.FindByIdAsync(id);
            return await _userManager.DeleteAsync(role);
        }

        public async Task<IEnumerable<AppUser>> GetAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();

                var paramaters = new DynamicParameters();
                var result = await conn.QueryAsync<AppUser>("Get_User_All", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<PagedResult<AppUser>> GetPagingAsync(string keyword, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();

                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var result = await conn.QueryAsync<AppUser>("Get_User_AllPaging", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                int totalRow = paramaters.Get<int>("@totalRow");

                var pagedResult = new PagedResult<AppUser>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                return pagedResult;
            }
        }

        public async Task<IList<string>> GetUserRolesAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var model = await _userManager.GetRolesAsync(user);
            return model;

        }

        public async Task RemoveRoleToUserAsync(Guid id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var roleId = await connection.ExecuteScalarAsync<Guid?>("SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = @normalizedName", new { normalizedName = roleName.ToUpper() });
                if (roleId.HasValue)
                    await connection.ExecuteAsync($"DELETE FROM [AspNetUserRoles] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}", new { userId = user.Id, roleId });
            }
        }

        public async Task<IdentityResult> UpdateAsync(Guid id, AppUser user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
