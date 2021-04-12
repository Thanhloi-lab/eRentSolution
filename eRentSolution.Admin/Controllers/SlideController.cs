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

        public SlideController(IConfiguration configuration
            , ICategoryApiClient categoryApiClient
            , ISlideApiClient slideApiClient
            , IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
            _slideApiClient = slideApiClient;
            userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetSlidePagingRequest()
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

            var slides = await _slideApiClient.GetPagings(request, SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            return View(slides);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slide = await _slideApiClient.GetById(id, SystemConstant.AppSettings.TokenWebApp);
            if(slide==null)
            {
                TempData["failResult"] = "Slide không tồn tại";
                return RedirectToAction("index");
            }
            var slideViewModel = new SlideUpdateRequest()
            {
                Id = id,
                Name = slide.Name,
                Description = slide.Description,
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
            if (result)
            {
                TempData["result"] = "Chỉnh sửa slide thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Chỉnh sửa slide thất bại");
            return View(request);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new SlideDeleteRequest()
            {
                Id = id
            });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(SlideDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _slideApiClient.DeleteSlide(request, SystemConstant.AppSettings.TokenAdmin, Guid.Parse(userId));

            if (result)
            {
                TempData["result"] = "Xóa slide thành công";
                return RedirectToAction("Index");
            }
            TempData["failResult"] = "Xóa slide không thành công";
            return View(request.Id);
        }
        //[HttpGet]
        //public async Task<IActionResult> Details(int id)
        //{
        //    var result = await _slideApiClient.GetById(id, SystemConstant.AppSettings.TokenWebApp);
        //    return View(result);
        //}
    }
}
