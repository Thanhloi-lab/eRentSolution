using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eRentSolution.WebApp.Controllers
{

    //[Authorize]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string userId;
        private readonly string token;
        public UserController(IUserApiClient userApiClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            token = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.TokenAdmin);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //var reusult = User.IsInRole(SystemConstant.AppSettings.AdminRole);
            if (!id.ToString().Equals(userId))
            {
                TempData["FailResult"] = "Không thể chỉnh sửa tài khoản";
                return View(id);
            }
            var target = await _userApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (target != null)
            {
                var user = target;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = id
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.Update(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Chỉnh sửa thành công";
                return RedirectToAction("index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public IActionResult ChangePassword(Guid id)
        {
            return View(new UserUpdatePasswordRequest()
            {
                Id = id
            });
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserUpdatePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.UpdatePassword(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Chỉnh sửa mật khẩu thành công";
                return RedirectToAction("index");
            }
            else
            {
                TempData["FailResult"] = "Chỉnh sửa mật khẩu không thành công";
                return RedirectToAction("index");
            }
            return View(request);
        }
    }
}
