using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRentSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        public readonly IProductApiClient _productApiClient;
        public readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Detail(int id, string languageId)
        {
            var product = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenWebApp);
            var categories = await _categoryApiClient.GetAllCategoryByProductId(id, SystemConstant.AppSettings.TokenWebApp);
            if (categories == null)
            {
                categories = new List<CategoryViewModel>();
                var category = new CategoryViewModel()
                {
                    Id = -1,
                    Name = "N/A",
                };
                categories.Add(category);
            }
            var products = new List<ProductViewModel>();
            products.Add(product);
            products = await GetProductImages(products);
            product = products.ElementAt(0);
            return View(new ProductDetailViewModels()
            {
                
                Categories = categories,
                Product = product
            });
        }

        public async Task<IActionResult> Category(int id, int page = 1, int pageSize = 10)
        {
            var products = await _productApiClient.GetPagings(new GetProductPagingRequest()
            {
                CategoryId = id,
                PageIndex = page,
                PageSize = pageSize
            }, SystemConstant.AppSettings.TokenWebApp);
            products.Items = await GetProductImages(products.Items);
            return View(new ProductCategoryViewModel()
            {
                Category = await _categoryApiClient.GetById(id, SystemConstant.AppSettings.TokenWebApp),
                Products = products
            });
        }
        public async Task<List<ProductViewModel>> GetProductImages(List<ProductViewModel> products)
        {
            foreach (var item in products)
            {
                var images = await _productApiClient.GetListImages(item.Id, SystemConstant.AppSettings.TokenWebApp);
                if (images != null)
                {
                    if (images.Count > 0)
                    {
                        foreach (var image in images)
                        {
                            if (image.IsDefault == true)
                                item.ThumbnailImage = image.ImagePath;
                        }
                        if (item.ThumbnailImage == null)
                        {
                            item.ThumbnailImage = images.ElementAt(0).ImagePath;
                        }
                    }
                }
            }
            return products;
        }
    }
}
