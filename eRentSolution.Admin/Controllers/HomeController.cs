﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eRentSolution.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using eRentSolution.AdminApp.Controllers;
using eRentSolution.Integration;
using System.Threading.Tasks;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.Utilities.Constants;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using eRentSolution.AdminApp.Models;

namespace eRentSolution.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly string userId;
        public HomeController(ILogger<HomeController> logger
            , IHttpContextAccessor httpContextAccessor,
            IProductApiClient productApiClient,
            ICategoryApiClient categoryApiClient)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _categoryApiClient = categoryApiClient;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index(bool? isStatisticMonth)
        {
            var request = new GetProductPagingRequest()
            {
                PageIndex = 1,
                PageSize = 10,
                IsGuess = false,
                IsStatisticMonth = false
            };
            if (isStatisticMonth != null)
            {
                request.IsStatisticMonth = isStatisticMonth;
            }
            var products = await _productApiClient.GetPagings(request, SystemConstant.AppSettings.TokenAdmin);
            var statisticUserProducts = await _productApiClient.GetStatisticUserProduct(request, SystemConstant.AppSettings.TokenAdmin);
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
            return View(new HomeViewModel()
            {
                StatisticProducts = products.ResultObject,
                StatisticUserProducts = statisticUserProducts.ResultObject
            });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
