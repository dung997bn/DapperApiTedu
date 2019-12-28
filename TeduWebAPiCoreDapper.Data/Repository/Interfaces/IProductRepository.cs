using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeduWebAPiCoreDapper.Data.Models;
using TeduWebAPiCoreDapper.Untilities.Dtos;

namespace TeduWebAPiCoreDapper.Data.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(string culture);
        Task<Product> GetByIdAsync(int id, string culture);
        Task<PagedResult<Product>> GetPagingAsync(string keyword, int categoryId, int pageIndex, int pageSize, string culture);
        Task<int> CreateAsync(Product product, string culture);
        Task UpdateAsync(int id, Product product, string culture);
        Task DeleteAsync(int id);

        Task<IEnumerable<JoinTable>> GetJoinTables(string culture);
    }
}
