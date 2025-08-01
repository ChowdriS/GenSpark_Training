using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{page}")]
        public async Task<ActionResult> Index(int? page, [FromQuery] int? pageSize)
        {
            try
            {
                int pageNumber = page ?? 1;
                int Size = pageSize ?? 3;
                var catList = await _categoryService.GetCategoryList(pageNumber, Size);
                return Ok(catList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("create")]
        
        public async Task<ActionResult> Create(CategoryRequestDTO category)
        {
            try
            {
                var Category = await _categoryService.AddCategory(category);
                return Ok(Category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("edit/{id}")]
        
        public async Task<ActionResult> Edit(int id, CategoryRequestDTO category)
        {
            try
            {
                var Category = await _categoryService.UpdateCategory(id, category);
                return Ok(Category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var Category = await _categoryService.GetCategoryById(id);
                return Ok(Category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var Category = await _categoryService.DeleteCategoryById(id);
                return Ok(Category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
