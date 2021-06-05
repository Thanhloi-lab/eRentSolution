﻿using eRentSolution.Integration;
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

    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string userId;
        private readonly string token;
        public UserController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            ISlideApiClient slideApiClient,
            IHttpContextAccessor httpContextAccessor,
            IUserApiClient userApiClient) : base(productApiClient, configuration, categoryApiClient, slideApiClient, httpContextAccessor, userApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            token = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.TokenWebApp);
        }
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var result = await _userApiClient.GetById(Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if(!result.IsSuccessed)
            {
                return RedirectToAction("index","home");
            }
            return View(result.ResultObject);
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var target = await _userApiClient.GetById(Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (target.IsSuccessed)
            {
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = target.ResultObject.Dob,
                    Email = target.ResultObject.Email,
                    FirstName = target.ResultObject.FirstName,
                    LastName = target.ResultObject.LastName,
                    PhoneNumber = target.ResultObject.PhoneNumber,
                    Id = Guid.Parse(userId)
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
            var result = await _userApiClient.Update(request, SystemConstant.AppSettings.TokenWebApp);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("details");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new UserUpdatePasswordRequest()
            {
                Id = Guid.Parse(userId)
            });
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserUpdatePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.UpdatePassword(request, SystemConstant.AppSettings.TokenWebApp);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Details");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditAvatar(Guid id)
        {
            var target = await _userApiClient.GetById(Guid.Parse(userId), SystemConstant.AppSettings.TokenWebApp);
            if (target.IsSuccessed)
            {
                var updateRequest = new UserAvatarUpdateRequest()
                {
                    Id = target.ResultObject.Id,
                };
                return View(updateRequest);
            }
            ModelState.AddModelError("", target.Message);
            return RedirectToAction("Details");
        }
        [HttpPost]
        public async Task<IActionResult> EditAvatar(UserAvatarUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View(request);
            }

            var result = await _userApiClient.UpdateAvatar(request, SystemConstant.AppSettings.TokenWebApp);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("index", "home");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> ActivityLog(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new UserActivityLogRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Id = Guid.Parse(userId)
            };
            ViewBag.keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["Result"];
            }
            var data = await _userApiClient.GetUserActivities(request, SystemConstant.AppSettings.TokenWebApp);
            return View(data.ResultObject);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            request.CurrentDomain = _configuration["CurrentDomain"];
            var result = await _userApiClient.ForgotPassword(request);
            if (result.IsSuccessed == true)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index", "Login");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordByEmail()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordByEmail(UserResetPasswordByEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View(request);
            }

            var result = await _userApiClient.ResetPasswordByEmail(request, SystemConstant.AppSettings.TokenWebApp);
            if (result.IsSuccessed == true)
            {
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> SendConfirmEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendConfirmEmail(SendConfirmEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View(request);
            }

            var result = await _userApiClient.SendConfirmEmail(request, SystemConstant.AppSettings.TokenWebApp);
            if (result.IsSuccessed == true)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View(request);
            }

            var result = await _userApiClient.ConfirmEmail(request, SystemConstant.AppSettings.TokenWebApp);
            if (result.IsSuccessed == true)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        public IActionResult Forbidden()
        {
            TempData["FailResult"] = "Thao tác không hợp lệ";
            return RedirectToAction("Index");
        }
    }
}
