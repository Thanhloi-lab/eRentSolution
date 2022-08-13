using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eRentSolution.AdminApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        public readonly ICategoryApiClient _categoryApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string userId;

        public CategoryController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            ISlideApiClient slideApiClient,
            IHttpContextAccessor httpContextAccessor,
            IUserApiClient userApiClient)
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

            return View(products.ResultObject);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> EditImage(int id)
        {
            var target = await _categoryApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (target.IsSuccessed && target.ResultObject != null)
            {
                var updateRequest = new CategoryImageUpdateRequest()
                {
                    CategoryId = id,
                    CategoryOldImagePath = target.ResultObject.Image,
                    CategoryName = target.ResultObject.Name
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> EditImage(CategoryImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _categoryApiClient.UpdateImage(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("index");
            }
            TempData["failResult"] = result.Message;
            return View(request);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var target = await _categoryApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (target.IsSuccessed && target.ResultObject != null)
            {
                return View(new CategoryStatusRequest()
                {
                    Id = id,
                    CategoryImagePath = target.ResultObject.Image,
                    CategoryName = target.ResultObject.Name
                });
            }
            return RedirectToAction("index");
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(CategoryStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _categoryApiClient.DeteleCategory(request, SystemConstant.AppSettings.TokenAdmin);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public IActionResult CreateCategory(int? id)
        {
            return View(new CategoryCreateRequest()
            {
                ParentId = id
            });
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _categoryApiClient.CreateCategory(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public IActionResult UpdateCategory(int id, string name)
        {
            return View(new CategoryUpdateRequest()
            {
                CategoryId = id,
                CategoryName = name
            });
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _categoryApiClient.UpdateCategory(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _categoryApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                return View(result.ResultObject);
            }

            ModelState.AddModelError("", result.Message);
            return RedirectToAction("Index");
        }
    }
}
