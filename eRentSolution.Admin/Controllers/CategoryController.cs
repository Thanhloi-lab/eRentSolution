﻿using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eRentSolution.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        public readonly ICategoryApiClient _categoryApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string userId;

        public CategoryController(ICategoryApiClient categoryApiClient, IHttpContextAccessor httpContextAccessor)
        {
            _categoryApiClient = categoryApiClient;
            _httpContextAccessor = httpContextAccessor;
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetCategoryPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }

            var products = await _categoryApiClient.GetPagings(request, SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Keyword = keyword;


            return View(products);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> EditImage(int id)
        {
            var target = await _categoryApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (target != null)
            {
                var updateRequest = new CategoryImageUpdateRequest()
                {
                    CategoryId = id
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> EditImage(CategoryImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _categoryApiClient.UpdateImage(request, SystemConstant.AppSettings.TokenAdmin);
            if (result)
            {
                TempData["result"] = "Update category successful";
                return RedirectToAction("index");
            }
            ModelState.AddModelError("", result.ToString());
            return View(request);
        }
    }
}