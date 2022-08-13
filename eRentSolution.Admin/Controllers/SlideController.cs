using eRentSolution.Data.Enums;
using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eRentSolution.AdminApp.Controllers
{
    [Authorize]
    public class SlideController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string userId;

        public SlideController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            ISlideApiClient slideApiClient,
            IHttpContextAccessor httpContextAccessor,
            IUserApiClient userApiClient)
        {
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
            _slideApiClient = slideApiClient;
            _httpContextAccessor = httpContextAccessor;
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public async Task<IActionResult> Index(string keyword, int? status, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetSlidePagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Status = status,
                IsGuess = false,
                BaseAddress = _configuration["WebAppDomain"]
            };

            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }

            var slides = await _slideApiClient.GetPagings(request, SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Keyword = keyword;

            var viewBagStatus = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Hoạt động",
                    Value = "1",
                    Selected = status.HasValue && status.Value.ToString() == "1"
                },
                new SelectListItem()
                {
                    Text = "Không Hoạt động",
                    Value = "0",
                    Selected = status.HasValue && status.Value.ToString() == "0"
                }
            };

            ViewBag.Status = viewBagStatus;
            return View(slides.ResultObject);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slide = await _slideApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if(!slide.IsSuccessed)
            {
                TempData["failResult"] = slide.Message;
                return RedirectToAction("index");
            }
            var slideViewModel = new SlideUpdateRequest()
            {
                Id = id,
                Name = slide.ResultObject.Name,
                Description = slide.ResultObject.Description,
            };
            return View(slideViewModel);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] SlideUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _slideApiClient.UpdateSlide(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _slideApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                return View(new SlideStatusRequest()
                {
                    Id = id,
                    SlideImagePath = result.ResultObject.FilePath
                });
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(SlideStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _slideApiClient.DeleteSlide(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request.Id);
        }
        [HttpGet]
        public async Task<IActionResult> Hide(int id)
        {
            var result = await _slideApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                return View(new SlideStatusRequest()
                {
                    Id = id,
                    SlideImagePath = result.ResultObject.FilePath
                });
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> Hide(SlideStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _slideApiClient.HideSlide(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request.Id);
        }
        [HttpGet]
        public async Task<IActionResult> Show(int id)
        {
            var result = await _slideApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                return View(new SlideStatusRequest()
                {
                    Id = id,
                    SlideImagePath = result.ResultObject.FilePath
                });
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> Show(SlideStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _slideApiClient.ShowSlide(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request.Id);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _slideApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if(result.IsSuccessed)
                return View(result.ResultObject);
            TempData["failResult"] = result.Message;
            return RedirectToAction("intdex");
        }
        [HttpGet]
        public IActionResult CreateSlide()
        {
            return View(new SlideCreateRequest()
            {
                ProductId = 0
            });
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateSlide([FromForm] SlideCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            request.ProductUrl = request.ProductUrl.ToLower();
            string formatAdmin = _configuration["CurrentDomain"] + "/product/details/" + "([0-9]+)";
            string formatWebApp = _configuration["WebAppDomain"] + "/product/details/" + "([0-9]+)";
            formatAdmin = formatAdmin.Split("https://")[1].ToLower();
            formatWebApp = formatWebApp.Split("https://")[1].ToLower();
            int id = 0;
            bool isContainHttps = request.ProductUrl.Contains("https://");
            if(isContainHttps)
            {
                request.ProductUrl = request.ProductUrl.Split("https://")[1];
            }    
            if (Regex.IsMatch(request.ProductUrl, formatAdmin) || Regex.IsMatch(request.ProductUrl, formatWebApp))
            {
                string[] splitedUrl = request.ProductUrl.Split("/");
                id = int.Parse(splitedUrl[splitedUrl.Length-1]);
            }
            request.ProductId = id;
            request.ProductUrl = _configuration["WebAppDomain"] + "/product/details/";

            var result = await _slideApiClient.CreateSlide(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
    }
}
