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
using System.Threading.Tasks;

namespace eRentSolution.AdminApp.Controllers
{
    public class StatisticController : BaseController
    {
        private readonly IProductApiClient _productApiClient;
        //private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string userId;
        //private readonly string userInfoId;
        public StatisticController(IProductApiClient productApiClient,
            IConfiguration configuration,
            IUserApiClient userApiClient,
            ICategoryApiClient categoryApiClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            //_userApiClient = userApiClient;
            _httpContextAccessor = httpContextAccessor;
            _categoryApiClient = categoryApiClient;
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, bool? isStatisticMonth, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetProductPagingRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId,
                IsGuess = false,
                IsStatisticMonth = false
            };
            if(isStatisticMonth!=null)
            {
                request.IsStatisticMonth = isStatisticMonth;
            }    
            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }

            var products = await _productApiClient.GetPagings(request, SystemConstant.AppSettings.TokenAdmin);

            var categories = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Categories = categories.ResultObject.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });

            var viewbagStatistic = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Theo năm",
                    Value = "false",
                    Selected = isStatisticMonth.HasValue && isStatisticMonth.Value == false
                },
                new SelectListItem()
                {
                    Text = "Theo tháng",
                    Value = "true",
                    Selected = isStatisticMonth.HasValue && isStatisticMonth.Value == true
                }
            };

            ViewBag.Statistic = viewbagStatistic;
            return View(products.ResultObject);
        }

        [HttpGet]
        public async Task<IActionResult> UserProduct(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetProductPagingRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword,
            };
            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }
            
            var statistic = await _productApiClient.GetStatisticUserProduct(request, SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Keyword = keyword;
            return View(statistic.ResultObject);
        }
    }
}
