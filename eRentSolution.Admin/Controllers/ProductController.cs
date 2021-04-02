using eRentSolution.Integration;
using eRentSolution.ViewModels.Catalog.Products;
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
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string userId;
        private readonly string userInfoId;
        public ProductController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            userInfoId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Actor).Value;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 3)
        {
            var request = new GetProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };

            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }

            var products = await _productApiClient.GetPagings(request);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll();
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _productApiClient.GetById(id);
            return View(result);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new ProductDeleteRequest() { 
                Id = id
            });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, bool isSuccess=false)
        {
            if (!ModelState.IsValid)
                return View();
            int infoId;
            if(int.TryParse(userInfoId, out infoId))
                isSuccess = await _productApiClient.DeleteProduct(id, infoId);
            
            if (isSuccess)
            {
                TempData["result"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }
            TempData["failResult"] = "Xóa sản phẩm không thành công";
            return View(id);
        }
    }
}
