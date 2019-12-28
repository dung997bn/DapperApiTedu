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
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly string _connectionString;
        private readonly IRoleRepository _roleRepository;

        public RoleController(RoleManager<AppRole> roleManager, IConfiguration configuration, IRoleRepository roleRepository)
        {
            _roleManager = roleManager;
            _connectionString = configuration.GetConnectionString("DbConnectionString");
            _roleRepository = roleRepository;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _roleRepository.GetAsync();
            return Ok(result);

        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _roleRepository.GetByIdAsync(id);
            return Ok(result);

        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(string keyword, int pageIndex, int pageSize)
        {
            var result = await _roleRepository.GetPagingAsync(keyword, pageIndex, pageSize);
            return Ok(result);
        }

        // POST: api/Role
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] AppRole role)
        {
            var result = await _roleRepository.CreateAsync(role);
            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }

        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([Required]Guid id, [FromBody] AppRole role)
        {
            role.Id = id;
            var result = await _roleRepository.UpdateAsync(id, role);
            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _roleRepository.DeleteAsync(id);
            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }
    }
}