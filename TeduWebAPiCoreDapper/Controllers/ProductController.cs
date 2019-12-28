using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TeduWebAPiCoreDapper.Data.Models;
using TeduWebAPiCoreDapper.Data.Repository;
using TeduWebAPiCoreDapper.Data.Repository.Interfaces;
using TeduWebAPiCoreDapper.Extensions;
using TeduWebAPiCoreDapper.Filters;
using TeduWebAPiCoreDapper.Models;
using TeduWebAPiCoreDapper.Resources;
using TeduWebAPiCoreDapper.Untilities.Dtos;

namespace TeduWebAPiCoreDapper.Controllers
{

    [Route("api/{culture}/[controller]")]
    [ApiController]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class ProductController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<ProductController> _logger;
        private readonly IStringLocalizer<ProductController> _localizer;
        private readonly LocService _locService;
        private readonly IProductRepository _productRepository;
        public ProductController(IConfiguration configuration, ILogger<ProductController> logger,
            IStringLocalizer<ProductController> localizer, LocService locService ,IProductRepository productRepository)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
            _logger = logger;
            _localizer = localizer;
            _locService = locService;
            _productRepository = productRepository;
        }
        // GET: api/Product
        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            //string text = _localizer["Test"];
            //string test = _locService.GetLocalizedHtmlString("ForgotPassword");
            return await _productRepository.GetAllAsync(CultureInfo.CurrentCulture.Name);
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Product> Get(int id)
        {
            return await _productRepository.GetByIdAsync(id, CultureInfo.CurrentCulture.Name);
        }
        [HttpGet("paging", Name = "GetPaging")]
        public async Task<PagedResult<Product>> GetPaging(string keyword, int categoryId, int pageIndex, int pageSize)
        {
            return await _productRepository.GetPagingAsync(keyword, categoryId, pageIndex, pageSize, CultureInfo.CurrentCulture.Name);
        }

        // POST: api/Product
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            var newId= await _productRepository.CreateAsync(product, CultureInfo.CurrentCulture.Name);
            return Ok(newId);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            await _productRepository.UpdateAsync(id, product, CultureInfo.CurrentCulture.Name);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.DeleteAsync(id);
            return Ok();
        }


        [HttpGet("JoinTable")]
        public async Task<IEnumerable<JoinTable>> GetJoinTable()
        {
            return await _productRepository.GetJoinTables(CultureInfo.CurrentCulture.Name);
        }


    }
}
