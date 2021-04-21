using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eRentSolution.WebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        public readonly IProductApiClient _productApiClient;
        public readonly ICategoryApiClient _categoryApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string userId;

        public ProductController(IProductApiClient productApiClient
            , ICategoryApiClient categoryApiClient
            , IHttpContextAccessor httpContextAccessor)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _httpContextAccessor = httpContextAccessor;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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

            var products = await _productApiClient.GetPagings(request, SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            return View(products);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Detail(int id)
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
        [HttpGet]
        public async Task<IActionResult> MyProductDetail(int id)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (await _productApiClient.IsMyProduct(id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp) == false)
            {
                return RedirectToAction("Index", "Home");
            }
            var products = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenWebApp);
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> MyListProducts(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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

            var products = await _productApiClient.GetPageProductsByUserId(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            return View(products);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.CreateProduct(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);
            if (result)
            {
                TempData["result"] = "Tạo mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Tạo mới sản phẩm thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var product = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenWebApp);
            if (await _productApiClient.IsMyProduct(product.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp) == false)
            {
                return RedirectToAction("Index", "Home");
            }
            var productViewModel = new ProductUpdateRequest()
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Details = product.Details,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                IsFeatured = product.IsFeatured
            };
            return View(productViewModel);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (await _productApiClient.IsMyProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp) == false)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = await _productApiClient.UpdateProduct(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (result)
            {
                TempData["result"] = "Chỉnh sửa sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Chỉnh sửa sản phẩm thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (await _productApiClient.IsMyProduct(id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp) == false)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new ProductStatusRequest()
            {
                Id = id
            });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ProductStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (await _productApiClient.IsMyProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp) == false)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = await _productApiClient.DeleteProduct(request.Id, SystemConstant.AppSettings.TokenAdmin);

            if (result)
            {
                TempData["result"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }
            TempData["failResult"] = "Xóa sản phẩm không thành công";
            return View(request.Id);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Category(int categoryId, int page = 1, int pageSize = 10)
        {
            var products = await _productApiClient.GetPagings(new GetProductPagingRequest()
            {
                CategoryId = categoryId,
                PageIndex = page,
                PageSize = pageSize
            }, SystemConstant.AppSettings.TokenWebApp);
            products.Items = await GetProductImages(products.Items);
            return View(new ProductCategoryViewModel()
            {
                Category = await _categoryApiClient.GetById(categoryId, SystemConstant.AppSettings.TokenWebApp),
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
