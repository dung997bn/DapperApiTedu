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
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<IEnumerable<Product>> GetAllAsync(string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var param = new DynamicParameters();
                param.Add("@language", culture);
                var result = await conn.QueryAsync<Product>("Get_Product_All", param, null, null, CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<Product> GetByIdAsync(int id, string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var param = new DynamicParameters();
                param.Add("@id", id);
                param.Add("@language", culture);
                var result = await conn.QueryAsync<Product>("Get_Product_ById", param, null, null, CommandType.StoredProcedure);
                return result.SingleOrDefault();
            }
        }


        public async Task<PagedResult<Product>> GetPagingAsync(string keyword, int categoryId, int pageIndex, int pageSize, string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var param = new DynamicParameters();
                param.Add("@keyword", keyword);
                param.Add("@categoryId", categoryId);
                param.Add("@pageIndex", pageIndex);
                param.Add("@pageSize", pageSize);
                param.Add("@language", culture);
                param.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = await conn.QueryAsync<Product>("Get_Product_AllPaging", param, null, null, CommandType.StoredProcedure);
                int totalRow = param.Get<int>("@totalRow");
                var pagedResult = new PagedResult<Product>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize

                };
                return pagedResult;
            }
        }

        public async Task<int> CreateAsync(Product product, string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var param = new DynamicParameters();
                param.Add("@name", product.Name);
                param.Add("@description", product.Description);
                param.Add("@content", product.Content);
                param.Add("@seoAlias", product.SeoAlias);
                param.Add("@seoTitle", product.SeoTitle);
                param.Add("@seoKeyword", product.SeoKeyword);
                param.Add("@seoDescription", product.SeoDescription);
                param.Add("@sku", product.Sku);
                param.Add("@price", product.Price);
                param.Add("@isActive", product.isActive);
                param.Add("@imageUrl", product.ImageUrl);
                param.Add("@language", culture);
                param.Add("@categoryIds", product.CategoryIds);
                param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = await conn.ExecuteAsync("Create_Product", param, null, null, CommandType.StoredProcedure);
                int newId = param.Get<int>("@id");
                return newId;
            }
        }

        public async Task UpdateAsync(int id, Product product, string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var param = new DynamicParameters();
                param.Add("@id", id);
                param.Add("@name", product.Name);
                param.Add("@description", product.Description);
                param.Add("@content", product.Content);
                param.Add("@seoAlias", product.SeoAlias);
                param.Add("@seoTitle", product.SeoTitle);
                param.Add("@seoKeyword", product.SeoKeyword);
                param.Add("@seoDescription", product.SeoDescription);
                param.Add("@sku", product.Sku);
                param.Add("@price", product.Price);
                param.Add("@isActive", product.isActive);
                param.Add("@imageUrl", product.ImageUrl);
                param.Add("@language", culture);
                param.Add("@categoryIds", product.CategoryIds);
                await conn.ExecuteAsync("Update_Product", param, null, null, CommandType.StoredProcedure);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var param = new DynamicParameters();
                param.Add("@id", id);
                await conn.ExecuteAsync("Delete_Product_ById", param, null, null, CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<JoinTable>> GetJoinTables(string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var param = new DynamicParameters();
                param.Add("@language", culture);
                var result = await conn.QueryAsync<JoinTable>("GetJoin",param, null, null, CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
