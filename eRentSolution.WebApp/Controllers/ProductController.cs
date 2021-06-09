using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.ProductDetails;
using eRentSolution.ViewModels.Catalog.ProductImages;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
using eRentSolution.WebApp.Models;
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

namespace eRentSolution.WebApp.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        public readonly IProductApiClient _productApiClient;
        public readonly ICategoryApiClient _categoryApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IUserApiClient _userApiClient;
        private string userId;

        public ProductController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            ISlideApiClient slideApiClient,
            IHttpContextAccessor httpContextAccessor,
            IUserApiClient userApiClient) : base(productApiClient, configuration, categoryApiClient, slideApiClient, httpContextAccessor, userApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _httpContextAccessor = httpContextAccessor;
            _userApiClient = userApiClient;
        }
        #region -----PRODUCT-------
        [AllowAnonymous]
        public async Task<IActionResult> Index(string keyword, string address, int? categoryId, int? minPrice, int? maxPrice, int pageIndex = 1, int pageSize = 10)
        {
            if (address != null)
            {
                if (address.Contains(SystemConstant.DefautAddress))
                    address = null;
            }
            var request = new GetProductPagingRequest()
            {
                CategoryId = categoryId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword,
                Address = address,
                IsGuess = true,
                MaxPrice = maxPrice,
                MinPrice = minPrice
            };   
            var products = await _productApiClient.GetPagings(request, SystemConstant.AppSettings.TokenWebApp);
            ViewBag.Keyword = keyword;
            products.ResultObject.Items = await GetProductImages(products.ResultObject.Items);
            var categories = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenWebApp);
            ViewBag.Categories = categories.ResultObject.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });

            return View(products.ResultObject);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenWebApp);
            if (!product.IsSuccessed)
            {
                TempData["failResult"] = product.Message;
                return RedirectToAction("Index", "home");
            }
            var categories = await _categoryApiClient.GetAllCategoryByProductId(id, SystemConstant.AppSettings.TokenWebApp);
            if (!categories.IsSuccessed)
            {
                categories.ResultObject = new List<CategoryViewModel>();
                var category = new CategoryViewModel()
                {
                    Id = -1,
                    Name = "N/A",
                };
                categories.ResultObject.Add(category);
            }
            var products = new List<ProductViewModel>();
            products.Add(product.ResultObject);
            products = await GetProductImages(products);
            product.ResultObject = products.ElementAt(0);
            var user = await _userApiClient.GetUserByProductId(product.ResultObject.Id, SystemConstant.AppSettings.TokenWebApp);
            if(!user.IsSuccessed)
            {
                TempData["failResult"] = user.Message;
                return RedirectToAction("Index");
            }
            var addViewCount = await _productApiClient.AddViewcount(id, SystemConstant.AppSettings.TokenWebApp);
            if(!addViewCount.IsSuccessed)
            {
                TempData["failResult"] = user.Message;
                return RedirectToAction("Index");
            }
            return View(new ProductDetailViewModels()
            {
                Owner = user.ResultObject,
                Categories = categories.ResultObject,
                Product = product.ResultObject
            }) ;
        }
        [HttpGet]
        public async Task<IActionResult> MyProductDetail(int id)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }
            var isMyProduct = await _productApiClient.IsMyProduct(id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (!isMyProduct.IsSuccessed)
            {
                TempData["failResult"] = isMyProduct.Message;
                return RedirectToAction("Index", "home");
            }
            var product = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenWebApp);
            if (!product.IsSuccessed)
            {
                TempData["failResult"] = product.Message;
                return RedirectToAction("MyListProducts");
            }
            return View(product.ResultObject);
        }
        [HttpGet]
        public async Task<IActionResult> MyListProducts(string keyword, string address, int? categoryId, int? minPrice, int? maxPrice, int pageIndex = 1, int pageSize = 10)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (address != null)
            {
                if (address.Contains(SystemConstant.DefautAddress))
                    address = null;
            }
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

            var products = await _productApiClient.GetPageProductsByUserId(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenWebApp);
            ViewBag.Categories = categories.ResultObject.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            return View(products.ResultObject);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserListProducts(Guid ownerId, string keyword, string address, int? categoryId, int? minPrice, int? maxPrice, int pageIndex = 1, int pageSize = 10)
        {
            if (address != null)
            {
                if (address.Contains(SystemConstant.DefautAddress))
                    address = null;
            }
            var request = new GetProductPagingRequest()
            {
                CategoryId = categoryId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword,
                Address = address,
                IsGuess = true,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
            };

            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }

            var products = await _productApiClient.GetPageProductsByUserId(request, ownerId, SystemConstant.AppSettings.TokenWebApp);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenWebApp);
            ViewBag.Categories = categories.ResultObject.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            products.ResultObject.Items = await GetProductImages(products.ResultObject.Items);
            var user = await _userApiClient.GetById(ownerId, SystemConstant.AppSettings.TokenWebApp);
            return View(new UserListProductsViewModel() 
            { 
                Products = products.ResultObject,
                Owner = user.ResultObject,
            });
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categoryObj = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenWebApp);
            var categoryAssignRequest = new List<SelectItem>();
            foreach (var item in categoryObj.ResultObject)
            {
                categoryAssignRequest.Add(new SelectItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Selected = false
                });
            }
            return View(new ProductCreateRequest()
            {
                Categories = categoryAssignRequest
            }) ;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View(request);
            }
            
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.CreateProduct(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp); 
            if (result.IsSuccessed)
            {
                CategoryAssignRequest assignRequest = new CategoryAssignRequest(){
                    Categories = request.Categories,
                    Id = result.ResultObject
                };
                var assignResult = await _productApiClient.CategoryAssign(assignRequest.Id, assignRequest, SystemConstant.AppSettings.TokenWebApp);
                if(assignResult.IsSuccessed)
                {
                    TempData["result"] = "Thêm sản phẩm thành công";
                    return RedirectToAction("MyListProducts");
                }
                TempData["result"] = "Đã xảy ra lỗi trong quá trình thiết lập danh mục, vui lòng thiết lập lại sau";
                return RedirectToAction("MyListProducts");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var product = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenWebApp);
            if (!product.IsSuccessed)
            {
                TempData["failResult"] = product.Message;
                return RedirectToAction("Index");
            }
            var isMyProduct = await _productApiClient.IsMyProduct(product.ResultObject.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (!isMyProduct.IsSuccessed)
            {
                return RedirectToAction("Index", "Home");
            }
            var productViewModel = new ProductUpdateRequest()
            {
                Id = id,
                Name = product.ResultObject.Name,
                Address = product.ResultObject.Address,
                Description = product.ResultObject.Description,
                //Details = product.Details,
                SeoAlias = product.ResultObject.SeoAlias,
                SeoDescription = product.ResultObject.SeoDescription,
                SeoTitle = product.ResultObject.SeoTitle,
            };
            return View(productViewModel);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Dữ liệu hông hợp lệ");
                return View(request);
            }

            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var isMyProduct = await _productApiClient.IsMyProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (!isMyProduct.IsSuccessed)
            {
                TempData["failResult"] = isMyProduct.Message;
                return RedirectToAction("Index", "Home");
            }
            var result = await _productApiClient.UpdateProduct(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("MyListProducts");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var isMyProduct = await _productApiClient.IsMyProduct(id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (!isMyProduct.IsSuccessed)
            {
                TempData["failResult"] = isMyProduct.Message;
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
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ");
                return View(request);
            }
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var isMyProduct = await _productApiClient.IsMyProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (!isMyProduct.IsSuccessed)
            {
                TempData["failResult"] = isMyProduct.Message;
                return RedirectToAction("Index", "Home");
            }
            var result = await _productApiClient.DeleteProduct(request.Id, SystemConstant.AppSettings.TokenWebApp);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("MyListProducts");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Category(string keyword, string address, int categoryId, int? minPrice, int? maxPrice, int pageIndex = 1, int pageSize = 10)
        {
            var products = await _productApiClient.GetPagings(new GetProductPagingRequest()
            {
                CategoryId = categoryId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Address = address,
                IsGuess = true,
                Keyword = keyword,
                MaxPrice = maxPrice,
                MinPrice = minPrice
            }, SystemConstant.AppSettings.TokenWebApp);
            products.ResultObject.Items = await GetProductImages(products.ResultObject.Items);
            var category = await _categoryApiClient.GetById(categoryId, SystemConstant.AppSettings.TokenWebApp);
            if(!category.IsSuccessed)
            {
                TempData["failResult"] = category.Message;
                return RedirectToAction("Index", "home");
            }    
            return View(new ProductCategoryViewModel()
            {
                Category = category.ResultObject,
                Products = products.ResultObject
            });
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
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var isMyProduct = await _productApiClient.IsMyProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            var result = await _productApiClient.HideProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("MyListProducts");
            }
            ModelState.AddModelError("", result.Message);
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

            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var isMyProduct = await _productApiClient.IsMyProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            var result = await _productApiClient.ShowProduct(request.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("MyListProducts");
            }
            ModelState.AddModelError("", result.Message);
            return View(request.Id);
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
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View();
            }    

            var result = await _productApiClient.CategoryAssign(request.Id, request, SystemConstant.AppSettings.TokenWebApp);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("MyListProducts");
            }
            ModelState.AddModelError("", result.Message);
            var categoryAssign = GetCategoryAssignRequest(request.Id);
            return View(categoryAssign);
        }
        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            var productObj = await _productApiClient.GetById(id, SystemConstant.AppSettings.TokenWebApp);
            var categoryObj = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenWebApp);
            var categoryAssignRequest = new CategoryAssignRequest();
            foreach (var category in categoryObj.ResultObject)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Selected = productObj.ResultObject.Categories.Contains(category.Name)
                });
            }
            return categoryAssignRequest;
        }
        #endregion

        #region ------DETAIL------
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
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ");
                return View(request);
            }    
                
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.AddProductDetail(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return Redirect($"/product/MyProductDetail/{request.ProductId}");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> EditDetail(int productDetailId, int productId)
        {
            var productDetail = await _productApiClient.GetProductDetailById(productDetailId, SystemConstant.AppSettings.TokenWebApp);
            if(!productDetail.IsSuccessed)
            {
                ModelState.AddModelError("", productDetail.Message);
                RedirectToAction("Detail");
            }
            var productViewModel = new ProductDetailUpdateRequest()
            {
                Id = productDetailId,
                Detail = productDetail.ResultObject.Detail,
                Length = productDetail.ResultObject.Length,
                Width = productDetail.ResultObject.Width,
                ProductDetailName = productDetail.ResultObject.ProductDetailName,
                ProductId = productId,
                Stock = productDetail.ResultObject.Stock,
                OriginalPrice = productDetail.ResultObject.OriginalPrice,
                Price = productDetail.ResultObject.Price
            };
            return View(productViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditDetail([FromForm] ProductDetailUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ");
                return View(request);
            }    
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.UpdateDetail(request, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return Redirect($"/product/MyProductDetail/{request.ProductId}");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteDetail(int productDetailId, int productId)
        {
            var product = await _productApiClient.GetById(productId, SystemConstant.AppSettings.TokenWebApp);
            if(!product.IsSuccessed)
            {
                TempData["failResult"] = product.Message;
                return RedirectToAction("MyListProducts");
            }
            else
            {
                userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var isMyProduct = await _productApiClient.IsMyProduct(productId, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
                if (!isMyProduct.IsSuccessed)
                {
                    TempData["failResult"] = isMyProduct.Message;
                    return RedirectToAction("index");
                }
                if (product.ResultObject.ProductDetailViewModels.Count<2)
                {
                    TempData["failResult"] = "Không thể xóa chi tiết cuối cùng của sản phẩm";
                    return Redirect($"/product/MyProductDetail/{productId}");
                }
            }    
            return View(new ProductDetailDeleteRequest()
            {
                ProductId = productId,
                ProductDetailId = productDetailId
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDetail([FromForm] ProductDetailDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ");
                return View(request);
            }

            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.DeleteDetail(request.ProductDetailId, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return Redirect($"/product/MyProductDetail/{request.ProductId}");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        #endregion

        #region ------IMAGE------
        [HttpGet]
        public async Task<IActionResult> EditImage(int imageId, int productId)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var isMyProduct = await _productApiClient.IsMyProduct(productId, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (!isMyProduct.IsSuccessed)
            {
                TempData["failResult"] = isMyProduct.Message;
                return RedirectToAction("Index", "Home");
            }
            var image = await _productApiClient.GetImageById(imageId, SystemConstant.AppSettings.TokenWebApp);
            if(!image.IsSuccessed)
            {
                TempData["failResult"] = image.Message;
                return Redirect($"/product/MyProductDetail/{productId}");
            }
            var imageUpdateRequest = new ProductImageUpdateRequest()
            {
                ImageId = image.ResultObject.Id,
                OldImageUrl = image.ResultObject.ImagePath,
                ProductId = productId
            };
            return View(imageUpdateRequest);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditImage([FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ");
                return View(request);
            }

            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.UpdateImage(request, SystemConstant.AppSettings.TokenWebApp, Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return Redirect($"/product/MyProductDetail/{request.ProductId}");
            }

            ModelState.AddModelError("", result.ResultObject);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> AddImage(int productDetailId, int productId)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var isMyProduct = await _productApiClient.IsMyProduct(productId, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (!isMyProduct.IsSuccessed)
            {
                TempData["failResult"] = isMyProduct.Message;
                return RedirectToAction("Index", "Home");
            }
            return View(new ProductImageCreateRequest()
            {
                ProductDetailId = productDetailId,
                ProductId = productId
            });
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddImage([FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ");
                return View(request);
            }
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.AddImage(request, SystemConstant.AppSettings.TokenWebApp, Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return Redirect($"/product/MyProductDetail/{request.ProductId}");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
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
        [HttpGet]
        public async Task<IActionResult> DeleteImage(int productDetailId, int imageId)
        {
            var productDetail = await _productApiClient.GetProductDetailById(productDetailId, SystemConstant.AppSettings.TokenWebApp);
            var product = await _productApiClient.GetById(productDetail.ResultObject.ProductId, SystemConstant.AppSettings.TokenWebApp);
            if (!productDetail.IsSuccessed)
            {
                TempData["failResult"] = productDetail.Message;
                return Redirect($"/product/MyProductDetail/{product.ResultObject.Id}");
            }
            else if (!product.IsSuccessed)
            {
                TempData["failResult"] = productDetail.Message;
                return Redirect($"/product/MyProductDetail/{product.ResultObject.Id}");
            }

            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var isMyProduct = await _productApiClient.IsMyProduct(product.ResultObject.Id, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (!isMyProduct.IsSuccessed)
            {
                TempData["failResult"] = isMyProduct.Message;
                return RedirectToAction("Index", "Home");
            }
            return View(new ProductImageDeleteRequest()
            {
                ProductId = productDetail.ResultObject.ProductId,
                ProductDetailId = productDetailId,
                ImageId = imageId
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteImage([FromForm] ProductImageDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ");
                return View(request);
            }

            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _productApiClient.DeleteImage(request.ImageId, Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return Redirect($"/product/MyProductDetail/{request.ProductId}");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        #endregion

    }
}
