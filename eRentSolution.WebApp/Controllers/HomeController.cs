using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eRentSolution.WebApp.Models;
using eRentSolution.Integration;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace eRentSolution.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;

        public HomeController(ILogger<HomeController> logger,
            IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            ISlideApiClient slideApiClient,
            IHttpContextAccessor httpContextAccessor,
            IUserApiClient userApiClient) : base(productApiClient, configuration, categoryApiClient, slideApiClient, httpContextAccessor, userApiClient)
        {
            _logger = logger;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            GetProductPagingRequest request = new GetProductPagingRequest()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                IsGuess = true
            };
            var slides = await _slideApiClient.GetAll(SystemConstant.AppSettings.TokenWebApp);
            var featuredProducts = await _productApiClient.GetFeaturedProducts(request, SystemConstant.AppSettings.TokenWebApp);
            var pageProducts = await _productApiClient.GetPagings(request, SystemConstant.AppSettings.TokenWebApp);
            var viewModel = new HomeViewModel
            {
                Slides = slides.ResultObject,
                FeaturedProducts = featuredProducts.ResultObject,
                PageProducts = pageProducts.ResultObject 
            };
            viewModel.FeaturedProducts.Items = await GetProductImages(viewModel.FeaturedProducts.Items);
            viewModel.PageProducts.Items = await GetProductImages(viewModel.PageProducts.Items);
            return View(viewModel);
        }
        public async Task<List<ProductViewModel>> GetProductImages(List<ProductViewModel> products)
        {
            foreach (var item in products)
            {
                var images = await _productApiClient.GetListImages(item.Id, SystemConstant.AppSettings.TokenWebApp);
                if (images.IsSuccessed)
                {
                    if (images.ResultObject.Count > 0)
                    {
                        foreach (var image in images.ResultObject)
                        {
                            if (image.IsDefault == true)
                                item.ThumbnailImage = image.ImagePath;
                        }
                        if (item.ThumbnailImage == null)
                        {
                            item.ThumbnailImage = images.ResultObject.ElementAt(0).ImagePath;
                        }
                    }
                }
            }
            return products;
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
