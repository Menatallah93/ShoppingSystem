using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.DTO;
using ShoppingSystem.Models;
using ShoppingSystem.Repository;
using System;
using System.Web.Http.Cors;

namespace ShoppingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "http://localhost:4200/", headers: "*", methods: "*")]

    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        
        // POST: api/products
        [HttpPost("AddProduct")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO productdto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productRepository.CreateAsync(productdto);
                    return Ok("Successfully Created");
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        // PUT: api/products/{id}
       // [Authorize(Roles = "Admin, Manager")]
        [HttpPut("EditProduct/{id}")]
        public async Task<IActionResult> EditProduct(int id, [FromBody] ProductDTO productdto)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(id, productdto);
                return Ok("Sussecced");
            }
            return BadRequest(ModelState);
        }

        // PUT: api/products/archive/{id}
        [HttpPut("archive/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveProduct(int id)
        {
            await _productRepository.SoftDeleteItemAsync(id);
            return Ok("Archived Item Is Done");
        }

        [HttpPut("ArchiveMultipleProducts")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveMultipleProducts([FromBody] IEnumerable<int> productIds)
        {
            await _productRepository.SoftDeleteItemsAsync(productIds);
            return Ok("Archived Items Are Done");
        }


        // GET: api/products
        [HttpGet("GetAllProductsByCategoryID")]
        public async Task<IActionResult> GetProducts(int categoryID)
        {
            var products = await _productRepository.GetCategoryWithProducts(categoryID);

            return Ok(products);
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();

            return Ok(products);
        }

        // GET: api/products/searchByName/{name}
        [HttpGet("searchByName/{name}")]
        public async Task<IActionResult> SearchByName(string name)
        {
            List<Product> products = await _productRepository.SearchByName(name);

            return Ok(products);
        }

        // GET: api/products/searchByDate/{date}
        [HttpGet("searchByDate/{date}")]
        public async Task<IActionResult> SearchByDate(DateTime date)
        {
           List<Product> products= await _productRepository.SearchByDate(date);

            return Ok(products);
        }
    }
}
