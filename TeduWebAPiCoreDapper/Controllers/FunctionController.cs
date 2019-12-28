using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TeduWebAPiCoreDapper.Data.Models;
using TeduWebAPiCoreDapper.Data.Repository.Interfaces;
using TeduWebAPiCoreDapper.Filters;

namespace TeduWebAPiCoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionController : ControllerBase
    {

        private readonly string _connectionString;
        private readonly IFunctionRepository _functionRepository;

        public FunctionController(IConfiguration configuration, IFunctionRepository functionRepository)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
            _functionRepository = functionRepository;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _functionRepository.GetAsync();
            return Ok(result);
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _functionRepository.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(string keyword, int pageIndex, int pageSize)
        {
            var result = await _functionRepository.GetPagingAsync(keyword,pageIndex,pageSize);
            return Ok(result);
        }

        // POST: api/Role
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] Function function)
        {
            await _functionRepository.CreateAsync(function);
            return Ok();
        }

        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([Required]string id, [FromBody] Function function)
        {
            await _functionRepository.UpdateAsync(id, function);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _functionRepository.DeleteAsync(id);
            return Ok();
        }
    }
}