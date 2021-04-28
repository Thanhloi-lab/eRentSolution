using eRentSolution.Application.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRentSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();
            if(categories.IsSuccessed)
                return Ok(categories);
            return BadRequest(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetById( id);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("productcategories/{productId}")]
        public async Task<IActionResult> GetAllByProductId(int productId)
        {
            var result = await _categoryService.GetAllCategoryByProductId(productId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("UpdateImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage([FromForm] CategoryImageUpdateRequest request)
        {
            var result = await _categoryService.UpdateImage(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("createCategory")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest request)
        {
            var result = await _categoryService.CreateCategory(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("updateCategory")]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryUpdateRequest request)
        {
            var result = await _categoryService.UpdateCategory(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("deleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var result = await _categoryService.DeleteCategory(categoryId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetCategoryPagingRequest request)
        {
            var result = await _categoryService.GetAllPaging(request);
            return Ok(result);
        }
    }
}
