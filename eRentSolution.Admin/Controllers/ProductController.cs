using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
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
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserApiClient _userApiClient;
        private readonly string userId;
        //private readonly string userInfoId;
        public ProductController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            ISlideApiClient slideApiClient,
            IHttpContextAccessor httpContextAccessor,
            IUserApiClient userApiClient)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _slideApiClient = slideApiClient;
            _userApiClient = userApiClient;
            _httpContextAccessor = httpContextAccessor;
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int? status, decimal? minPrice, decimal? maxPrice, string address, int pageIndex = 1, int pageSize = 10)
        {
            if (address != null)
            {
                if (address.Contains(SystemConstant.DefautAddress))
                    address = null;
            }    
            var request = new GetProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId,
                IsGuess = false,
                Address = address,
                MaxPrice = maxPrice,
                MinPrice = maxPrice,
                Status = status
            };

            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }

            var viewBagStatus = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Bị khóa",
                    Value = "1",
                    Selected = status.HasValue && status.Value.ToString() == "1"
                },
                new SelectListItem()
                {
                    Text = "Hoạt động",
                    Value = "2",
                    Selected = status.HasValue && status.Value.ToString() == "2"
                },
                new SelectListItem()
                {
                    Text = "Chờ kiểm duyệt",
                    Value = "3",
                    Selected = status.HasValue && status.Value.ToString() == "3"
                },
                new SelectListItem()
                {
                    Text = "Đang ẩn",
                    Value = "4",
                    Selected = status.HasValue && status.Value.ToString() == "4"
                }
            };

            ViewBag.Status = viewBagStatus;

            var products = await _productApiClient.GetPagings(request, SystemConstant.AppSettings.TokenAdmin);
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
        public async Task<IActionResult> Details(int id)
        {
            var result = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if(!result.IsSuccessed)
            {
                TempData["failResult"] = result.Message;
                return RedirectToAction("index");
            }    
            return View(result.ResultObject);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                return View(new ProductStatusRequest()
                {
                    Id = id,
                    ProductImagePath=result.ResultObject.ThumbnailImage,
                    ProductName = result.ResultObject.Name
                });
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ProductStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _productApiClient.DeleteProduct(request.Id, SystemConstant.AppSettings.TokenAdmin);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request.Id);
        }
        [HttpGet]
        public async Task<IActionResult> InActiveProduct(int id)
        {
            var result = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                return View(new ProductStatusRequest()
                {
                    Id = id,
                    ProductImagePath = result.ResultObject.ThumbnailImage,
                    ProductName = result.ResultObject.Name
                });
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> InActiveProduct(ProductStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _productApiClient.InActiveProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request.Id);
        }
        [HttpGet]
        public async Task<IActionResult> ActiveProduct(int id)
        {
            var result = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                return View(new ProductStatusRequest()
                {
                    Id = id,
                    ProductImagePath = result.ResultObject.ThumbnailImage,
                    ProductName = result.ResultObject.Name
                });
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> ActiveProduct(ProductStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _productApiClient.ActiveProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request.Id);
        }
        //[HttpGet]
        //public IActionResult CreateSlide(int id)
        //{
        //    return View(new SlideCreateRequest()
        //    {
        //        ProductId = id
        //    });
        //}
        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> CreateSlide([FromForm] SlideCreateRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return View();

        //    var result = await _slideApiClient.CreateSlide(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
        //    if (result.IsSuccessed)
        //    {
        //        TempData["result"] = result.ResultObject;
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", result.Message);
        //    return View(request);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateFeature(int id)
        //{
        //    if (!ModelState.IsValid)
        //        return View();
        //    var request = new FeatureProductRequest()
        //    {
        //        ProductId = id
        //    };
        //    var result = await _productApiClient.CreateFeature(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
        //    if (result.IsSuccessed)
        //    {
        //        TempData["result"] = result.ResultObject;
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", result.Message);
        //    return RedirectToAction("Details");
        //}
        //[HttpPost]
        //public async Task<IActionResult> DeleteFeature(int id)
        //{
        //    if (!ModelState.IsValid)
        //        return View();
        //    var request = new FeatureProductRequest()
        //    {
        //        ProductId = id
        //    };
        //    var result = await _productApiClient.DeleteFeature(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
        //    if (result.IsSuccessed)
        //    {
        //        TempData["result"] = result.ResultObject;
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", result.Message);
        //    return RedirectToAction("Details");
        //}

        [HttpGet]
        public async Task<IActionResult> UserProduct(Guid userId ,string keyword, string address, int? categoryId, int? minPrice, int? maxPrice, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetProductPagingRequest()
            {
                CategoryId = categoryId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword,
                Address = address,
                IsGuess = false,
                MaxPrice = maxPrice,
                MinPrice = minPrice
            };

            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }
            var user = await _userApiClient.GetById(userId, SystemConstant.AppSettings.TokenAdmin);
            if(!user.IsSuccessed)
            {
                ModelState.AddModelError("", user.Message);
                return RedirectToAction("Index");
            }
            var products = await _productApiClient.GetPageProductsByUserId(request, userId, SystemConstant.AppSettings.TokenAdmin);
            
            ViewBag.Keyword = keyword;
            ViewBag.UserName = user.ResultObject.UserName;
            ViewBag.UserId = user.ResultObject.Id;

            var categories = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Categories = categories.ResultObject.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            return View(products.ResultObject);
        }



        //[HttpGet]
        //public IActionResult AddDetail(int id)
        //{
        //    return View(new ProductDetailCreateRequest()
        //    {
        //        ProductId = id
        //    });
        //}
        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> AddDetail([FromForm] ProductDetailCreateRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return View();

        //    var result = await _productApiClient.AddProductDetail(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);
        //    if (result)
        //    {
        //        TempData["result"] = "Tạo mới sản phẩm thành công";
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", "Tạo mới sản phẩm thất bại");
        //    return View(request);
        //}
        //[HttpGet]
        //public async Task<IActionResult> EditDetail(int productDetailId)
        //{
        //    var productDetail = await _productApiClient.GetProductDetailById(productDetailId, SystemConstant.AppSettings.TokenAdmin);
        //    var productViewModel = new ProductDetailUpdateRequest()
        //    {
        //        Id = productDetailId,
        //        Detail = productDetail.Detail,
        //        Length = productDetail.Length,
        //        Width = productDetail.Width,
        //        ProductDetailName = productDetail.ProductDetailName,
        //        OriginalPrice = productDetail.OriginalPrice,
        //        Price = productDetail.Price,
        //        Stock = productDetail.Stock,
        //    };
        //    return View(productViewModel);
        //}
        //[HttpPost]
        //public async Task<IActionResult> EditDetail([FromForm] ProductDetailUpdateRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return View();

        //    var result = await _productApiClient.UpdateDetail(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);
        //    if (result)
        //    {
        //        TempData["result"] = "Chỉnh sửa sản phẩm thành công";
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", "Chỉnh sửa sản phẩm thất bại");
        //    return View(request);
        //}
        //[HttpGet]
        //public async Task<IActionResult> EditImage(int imageId)
        //{
        //    var image = await _productApiClient.GetImageById(imageId, SystemConstant.AppSettings.TokenAdmin);
        //    var imageUpdateRequest = new ProductImageUpdateRequest()
        //    {
        //        ImageId = image.Id,
        //        OldImageUrl = image.ImagePath
        //    };
        //    return View(imageUpdateRequest);
        //}
        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> EditImage([FromForm] ProductImageUpdateRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return View();

        //    var result = await _productApiClient.UpdateImage(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
        //    if (result.IsSuccessed)
        //    {
        //        TempData["result"] = result.ResultObject;
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", result.ResultObject);
        //    return View(request);
        //}
        //[HttpGet]
        //public IActionResult AddImage(int productDetailId)
        //{
        //    return View(new ProductImageCreateRequest()
        //    {
        //        ProductDetailId = productDetailId
        //    });
        //}
        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> AddImage([FromForm] ProductImageCreateRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return View();

        //    var result = await _productApiClient.AddImage(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
        //    if (result.IsSuccessed)
        //    {
        //        TempData["result"] = result.ResultObject;
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", result.ResultObject);
        //    return View(request);
        //}


        //    return categoryAssignRequest;
        //}
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return View();

        //    var result = await _productApiClient.CreateProduct(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);
        //    if (result)
        //    {
        //        TempData["result"] = "Tạo mới sản phẩm thành công";
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", "Tạo mới sản phẩm thất bại");
        //    return View(request);
        //}
        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var product = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
        //    var productViewModel = new ProductUpdateRequest()
        //    {
        //        Id = id,
        //        Name = product.Name,
        //        Description = product.Description,
        //        //Details = product.Details,
        //        SeoAlias = product.SeoAlias,
        //        SeoDescription = product.SeoDescription,
        //        SeoTitle = product.SeoTitle,
        //        Address = product.Address,
        //        IsFeatured = product.IsFeatured
        //    };
        //    return View(productViewModel);
        //}
        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return View();

        //    var result = await _productApiClient.UpdateProduct(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);
        //    if (result)
        //    {
        //        TempData["result"] = "Chỉnh sửa sản phẩm thành công";
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", "Chỉnh sửa sản phẩm thất bại");
        //    return View(request);
        //}
    }
}
