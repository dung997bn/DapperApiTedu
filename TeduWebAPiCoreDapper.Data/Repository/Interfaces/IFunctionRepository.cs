using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeduWebAPiCoreDapper.Data.Models;
using TeduWebAPiCoreDapper.Untilities.Dtos;

namespace TeduWebAPiCoreDapper.Data.Repository.Interfaces
{
    public interface IFunctionRepository
    {
        Task<IEnumerable<Function>> GetAsync();
        Task<Function> GetByIdAsync(string id);
        Task<PagedResult<Function>> GetPagingAsync(string keyword, int pageIndex, int pageSize);
        Task CreateAsync(Function function);
        Task UpdateAsync(string id, Function function);
        Task DeleteAsync(string id);
    }
}
