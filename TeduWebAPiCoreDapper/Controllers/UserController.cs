using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TeduWebAPiCoreDapper.Data.Models;
using TeduWebAPiCoreDapper.Data.Repository.Interfaces;
using TeduWebAPiCoreDapper.Filters;

namespace TeduWebAPiCoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly string _connectionString;
        private readonly IUserRepository _userRepository;

        public UserController(UserManager<AppUser> userManager, IConfiguration configuration, IUserRepository userRepository)
        {
            _userManager = userManager;
            _connectionString = configuration.GetConnectionString("DbConnectionString");
            _userRepository = userRepository;
        }

        // GET: api/Role
        [HttpGet]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, ActionCode.VIEW)]
        public async Task<IActionResult> Get()
        {
            var result = await _userRepository.GetAsync();
            return Ok(result);
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _userRepository.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(string keyword, int pageIndex, int pageSize)
        {
            var result = await _userRepository.GetPagingAsync(keyword, pageIndex, pageSize);
            return Ok(result);
        }

        // POST: api/Role
        [HttpPost]
        [ValidateModel]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, ActionCode.CREATE)]
        public async Task<IActionResult> Post([FromBody] AppUser user)
        {
            var result = await _userRepository.CreateAsync(user);
            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }

        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([Required]Guid id, [FromBody] AppUser user)
        {
            user.Id = id;
            var result = await _userRepository.UpdateAsync(id, user);
            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }


        [HttpPut("{id}/{roleName}/assign-to-roles")]
        public async Task<IActionResult> AssignToRoles([Required]Guid id, [Required]string roleName)
        {
            await _userRepository.AssignToRolesAsync(id, roleName);
            return Ok();

        }
        [HttpDelete("{id}/{roleName}/remove-roles")]
        [ValidateModel]
        public async Task<IActionResult> RemoveRoleToUser([Required]Guid id, [Required]string roleName)
        {
            await _userRepository.RemoveRoleToUserAsync(id, roleName);
            return Ok();
        }
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            await _userRepository.GetUserRolesAsync(id);
            return Ok();
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }
    }
}