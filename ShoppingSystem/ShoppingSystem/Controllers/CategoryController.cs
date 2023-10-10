using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingSystem.DTO;
using ShoppingSystem.Models;
using ShoppingSystem.Repository;
using System.Security.Claims;
using System.Web.Http.Cors;

namespace ShoppingSystem.Controllers
{
    //[Authorize (Roles = "Admin , Manger")]
    [EnableCors(origins: "http://localhost:4200/", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoryController(ICategoriesRepository categoriesRepository) 
        {
            _categoriesRepository = categoriesRepository;
        }

        [AllowAnonymous]
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetCategory()
        {
            return Ok(await _categoriesRepository.GetAllAsync());
        }

        // POST: api/products

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody]CategoriesDTO categoriesDTO)
        {
            try
            {
                 _categoriesRepository.CreateAsync(categoriesDTO);
                 return Ok("Successfully Created");  
            }
            catch
            {
                return BadRequest("An error occurred.");
            }
        }


        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCategory(int id, [FromBody] CategoriesDTO categoriesDTO)
        {
            if (ModelState.IsValid)
            {
               CategoriesDTO categories = await _categoriesRepository.UpdateAsync(id, categoriesDTO);
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}
