using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eRentSolution.AdminApp.Controllers
{
    public class FeaturedProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string userId;
        //private readonly string userInfoId;
        public FeaturedProductController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            ISlideApiClient slideApiClient,
            IHttpContextAccessor httpContextAccessor,
            IUserApiClient userApiClient)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _httpContextAccessor = httpContextAccessor;
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //userInfoId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Actor).Value;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int? categoryId, decimal? minPrice, decimal? maxPrice, string address, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId,
                IsGuess = false,
                Address = address,
                MaxPrice = maxPrice,
                MinPrice = maxPrice
            };

            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }

            var products = await _productApiClient.GetFeaturedProducts(request, SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Categories = categories.ResultObject.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            return View(products.ResultObject);
        }

        [HttpGet]
        public IActionResult CreateFeatured()
        {
            return View(new FeatureProductRequest()
            { 
                ProductId=0
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateFeatured(FeatureProductRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            request.Url = request.Url.ToLower();
            string formatAdmin = _configuration["CurrentDomain"] + "/product/details/" + "([0-9]+)";
            string formatWebApp = _configuration["WebAppDomain"] + "/product/details/" + "([0-9]+)";
            formatAdmin = formatAdmin.Split("https://")[1].ToLower();
            formatWebApp = formatWebApp.Split("https://")[1].ToLower();
            int id = 0;
            bool isContainHttps = request.Url.Contains("https://");
            if (isContainHttps)
            {
                request.Url = request.Url.Split("https://")[1];
            }
            if (Regex.IsMatch(request.Url, formatAdmin) || Regex.IsMatch(request.Url, formatWebApp))
            {
                string[] splitedUrl = request.Url.Split("/");
                id = int.Parse(splitedUrl[splitedUrl.Length - 1]);
            }

            request.ProductId = id;
            var result = await _productApiClient.CreateFeature(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            return View(new FeatureProductRequest()
            {
                ProductId = id
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFeature(FeatureProductRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _productApiClient.DeleteFeature(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return RedirectToAction("Details");
        }
    }
}
