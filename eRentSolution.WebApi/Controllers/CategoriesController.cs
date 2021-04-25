﻿using eRentSolution.Application.Catalog.Categories;
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
            var categories = await _categoryService.GetById( id);
            if (categories.IsSuccessed)
                return Ok(categories);
            return BadRequest(categories);
        }

        [HttpGet("productcategories/{productId}")]
        public async Task<IActionResult> GetAllByProductId(int productId)
        {
            var categories = await _categoryService.GetAllCategoryByProductId(productId);
            if (categories.IsSuccessed)
                return Ok(categories);
            return BadRequest(categories);
        }
        [HttpPut("UpdateImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage([FromForm] CategoryImageUpdateRequest request)
        {
            var categories = await _categoryService.UpdateImage(request);
            if (categories.IsSuccessed)
                return Ok(categories);
            return BadRequest(categories);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetCategoryPagingRequest request)
        {
            var product = await _categoryService.GetAllPaging(request);
            return Ok(product);
        }
    }
}
