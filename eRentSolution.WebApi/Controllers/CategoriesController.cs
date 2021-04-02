using eRentSolution.Application.Catalog.Categories;
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
            var products = await _categoryService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var categories = await _categoryService.GetById( id);
            return Ok(categories);
        }

        [HttpGet("productcategories/{productId}")]
        public async Task<IActionResult> GetAllByProductId(int productId)
        {
            var categories = await _categoryService.GetAllCategoryByProductId(productId);
            return Ok(categories);
        }
    }
}
