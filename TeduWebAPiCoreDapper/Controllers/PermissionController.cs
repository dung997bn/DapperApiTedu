using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TeduWebAPiCoreDapper.Data.Repository.Interfaces;
using TeduWebAPiCoreDapper.Data.ViewModels;
using TeduWebAPiCoreDapper.Extensions;


namespace TeduWebAPiCoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IPermissionRepository _permissionRepository;

        public PermissionController(IConfiguration configuration, IPermissionRepository permissionRepository)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
            _permissionRepository = permissionRepository;
        }

        [HttpGet("function-actions")]
        public async Task<IActionResult> GetAllWithPermission()
        {
            var result = await _permissionRepository.GetAllWithPermissionAsync();
                return Ok(result);  
        }

        [HttpGet("{role}/role-permissions")]
        public async Task<IActionResult> GetAllRolePermissions(Guid? role)
        {
            var result = await _permissionRepository.GetAllRolePermissionsAsync(role);
            return Ok(result);
        }

        [HttpPost("{role}/save-permissions")]
        public async Task<IActionResult> SavePermissions(Guid role, [FromBody]List<PermissionViewModel> permissions)
        {
            await _permissionRepository.SavePermissionsAsync(role,permissions);
            return Ok();
        }

        [HttpGet("functions-view")]
        public async Task<IActionResult> GetAllFunctionByPermission()
        {
            var result = await _permissionRepository.GetAllFunctionByPermissionAsync(User.GetUserId());
            return Ok(result);
        }
    }
}