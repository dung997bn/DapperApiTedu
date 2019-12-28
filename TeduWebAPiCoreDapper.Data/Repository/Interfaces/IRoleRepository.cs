using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeduWebAPiCoreDapper.Data.Models;
using TeduWebAPiCoreDapper.Untilities.Dtos;

namespace TeduWebAPiCoreDapper.Data.Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<AppRole>> GetAsync();
        Task<AppRole> GetByIdAsync(string id);
        Task<PagedResult<AppRole>> GetPagingAsync(string keyword, int pageIndex, int pageSize);
        Task<IdentityResult> CreateAsync(AppRole role);
        Task<IdentityResult> UpdateAsync(Guid id, AppRole role);
        Task<IdentityResult> DeleteAsync(string id);
    }
}
