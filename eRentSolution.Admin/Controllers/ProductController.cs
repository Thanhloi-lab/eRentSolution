using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
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
        private readonly string userId;
        //private readonly string userInfoId;
        public ProductController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            ISlideApiClient slideApiClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _slideApiClient = slideApiClient;
            _httpContextAccessor = httpContextAccessor;
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //userInfoId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Actor).Value;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
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
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenWebApp);
            return View(result);
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
            var product = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            var productViewModel = new ProductUpdateRequest()
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                //Details = product.Details,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                Address = product.Address,
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

            var result = await _productApiClient.UpdateProduct(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);
            if (result)
            {
                TempData["result"] = "Chỉnh sửa sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Chỉnh sửa sản phẩm thất bại");
            return View(request);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new ProductStatusRequest() { 
                Id = id
            });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ProductStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _productApiClient.DeleteProduct(request.Id, SystemConstant.AppSettings.TokenAdmin);
            
            if (result)
            {
                TempData["result"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }
            TempData["failResult"] = "Xóa sản phẩm không thành công";
            return View(request.Id);
        }
        [HttpGet]
        public IActionResult Hide(int id)
        {
            return View(new ProductStatusRequest()
            {
                Id = id
            });
        }
        [HttpPost]
        public async Task<IActionResult> Hide(ProductStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _productApiClient.HideProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);

            if (result)
            {
                TempData["result"] = "Ẩn sản phẩm thành công";
                return RedirectToAction("Index");
            }
            TempData["failResult"] = "Ẩn sản phẩm không thành công";
            return View(request.Id);
        }
        [HttpGet]
        public IActionResult Show(int id)
        {
            return View(new ProductStatusRequest()
            {
                Id = id
            });
        }
        [HttpPost]
        public async Task<IActionResult> Show(ProductStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _productApiClient.ShowProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);

            if (result)
            {
                TempData["result"] = "Hiện sản phẩm thành công";
                return RedirectToAction("Index");
            }
            TempData["failResult"] = "Hiện sản phẩm không thành công";
            return View(request.Id);
        }
        [HttpGet]
        public IActionResult CreateSlide(int id)
        {
            return View(new SlideCreateRequest()
            {
                ProductId = id
            }) ;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateSlide([FromForm]SlideCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _slideApiClient.CreateSlide(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
            if (result)
            {
                TempData["result"] = "Tạo mới slide thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Tạo mới slide thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var categoryAssignRequest = await GetCategoryAssignRequest(id);
            return View(categoryAssignRequest);
        }
        [HttpPost]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.CategoryAssign(request.Id, request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Assign role successfully";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            var categoryAssign = GetCategoryAssignRequest(request.Id);
            return View(categoryAssign);
        }
        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            var productObj = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            var categoryObj = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenAdmin);
            var categoryAssignRequest = new CategoryAssignRequest();
            foreach (var category in categoryObj)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Selected = productObj.Categories.Contains(category.Name)
                });
            }

            return categoryAssignRequest;
        }
        [HttpPost]
        public async Task<IActionResult> CreateFeature(int id)
        {
            if (!ModelState.IsValid)
                return View();
            var request = new FeatureProductRequest()
            {
                ProductId = id
            };
            var result = await _productApiClient.CreateFeature(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
            if (result)
            {
                TempData["result"] = "Thêm sản sản phẩm nổi bật thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản sản phẩm nổi bật thất bại");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            if (!ModelState.IsValid)
                return View();
            var request = new FeatureProductRequest()
            {
                ProductId = id
            };
            var result = await _productApiClient.DeleteFeature(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
            if (result)
            {
                TempData["result"] = "Xóa sản sản phẩm nổi bật thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa sản sản phẩm nổi bật thất bại");
            return RedirectToAction("Index");
        }
    }
}
