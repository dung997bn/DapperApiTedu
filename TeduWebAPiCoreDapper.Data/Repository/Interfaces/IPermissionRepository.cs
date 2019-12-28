using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeduWebAPiCoreDapper.Data.ViewModels;

namespace TeduWebAPiCoreDapper.Data.Repository.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<FunctionActionViewModel>> GetAllWithPermissionAsync();
        Task<IEnumerable<PermissionViewModel>> GetAllRolePermissionsAsync(Guid? role);
        Task SavePermissionsAsync(Guid role, List<PermissionViewModel> permissions);
        Task<IEnumerable<FunctionViewModel>> GetAllFunctionByPermissionAsync(Guid userId);
    }
}
