using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.ProductDetails;
using eRentSolution.ViewModels.Catalog.ProductImages;
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
        public readonly IUserApiClient _userApiClient;
        private string userId;

        public ProductController(IProductApiClient productApiClient
            , ICategoryApiClient categoryApiClient
            , IHttpContextAccessor httpContextAccessor,
            IUserApiClient userApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _httpContextAccessor = httpContextAccessor;
            _userApiClient = userApiClient;
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
            var user = await _userApiClient.GetUserByProductId(product.Id, SystemConstant.AppSettings.TokenWebApp);
            return View(new ProductDetailViewModels()
            {
                Owner = user,
                Categories = categories,
                Product = product
            }) ;
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
        public async Task<IActionResult> GetUserListProducts(Guid ownerId, string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
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

            var products = await _productApiClient.GetPageProductsByUserId(request, ownerId, SystemConstant.AppSettings.TokenWebApp);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            products.Items = await GetProductImages(products.Items);
            var user = await _userApiClient.GetById(ownerId, SystemConstant.AppSettings.TokenWebApp);
            return View(new UserListProductsViewModel() 
            { 
                Products = products,
                Owner = user,
            });
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
                return RedirectToAction("MyListProducts");
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
                //Details = product.Details,
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
        [HttpGet]
        public IActionResult AddDetail(int id)
        {
            return View(new ProductDetailCreateRequest()
            {
                ProductId = id
            });
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddDetail([FromForm] ProductDetailCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.AddProductDetail(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (result)
            {
                TempData["result"] = "Tạo mới sản phẩm thành công";
                return RedirectToAction("MyListProducts");
            }

            ModelState.AddModelError("", "Tạo mới sản phẩm thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> EditDetail(int productDetailId)
        {
            var productDetail = await _productApiClient.GetProductDetailById(productDetailId, SystemConstant.AppSettings.TokenWebApp);
            var productViewModel = new ProductDetailUpdateRequest()
            {
                Id = productDetailId,
                Detail = productDetail.Detail,
                Length = productDetail.Length,
                Width = productDetail.Width,
                ProductDetailName = productDetail.ProductDetailName
            };
            return View(productViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditDetail([FromForm] ProductDetailUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.UpdateDetail(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (result)
            {
                TempData["result"] = "Chỉnh sửa sản phẩm thành công";
                return RedirectToAction("MyListProducts");
            }

            ModelState.AddModelError("", "Chỉnh sửa sản phẩm thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> EditImage(int imageId)
        {
            var image = await _productApiClient.GetImageById(imageId, SystemConstant.AppSettings.TokenWebApp);
            var imageUpdateRequest = new ProductImageUpdateRequest()
            {
                ImageId = image.Id,
                OldImageUrl = image.ImagePath
            };
            return View(imageUpdateRequest);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditImage([FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.UpdateImage(request, SystemConstant.AppSettings.TokenWebApp, Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("MyListProducts");
            }

            ModelState.AddModelError("", result.ResultObject);
            return View(request);
        }
        [HttpGet]
        public IActionResult AddImage(int productDetailId)
        {
            return View(new ProductImageCreateRequest()
            {
                ProductDetailId = productDetailId
            });
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddImage([FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.AddImage(request, SystemConstant.AppSettings.TokenWebApp, Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("MyListProducts");
            }

            ModelState.AddModelError("", result.ResultObject);
            return View(request);
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
