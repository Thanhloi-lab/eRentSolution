﻿using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eRentSolution.AdminApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string userId;
        private readonly string token;
        public UserController(IUserApiClient userApiClient,
            IConfiguration configuration,
            IRoleApiClient roleApiClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
            _httpContextAccessor = httpContextAccessor;
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            token = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.Token) ;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            ViewBag.keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["Result"];
            }
            var data = await _userApiClient.GetUsersPaging(request);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete(SystemConstant.AppSettings.Token);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "login");
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> Create(UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "User registration is successful";
                return RedirectToAction("index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //var reusult = User.IsInRole(SystemConstant.AppSettings.AdminRole);
            if (!id.ToString().Equals(userId))
            {
                TempData["FailResult"] = "You cannot update others user";
                return View(id);
            }
            var target = await _userApiClient.GetById(id);
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
            var result = await _userApiClient.Update(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Update user successful";
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
            var result = await _userApiClient.UpdatePassword(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Update password successful";
                return RedirectToAction("index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public IActionResult ResetPassword(Guid id)
        {
            return View(new UserResetPasswordRequest()
            {
                Id = id,
                Token = token,
                NewPassword = SystemConstant.AppSettings.PasswordReseted
            });
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(UserResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.ResetPassword(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Update password successful";
                return RedirectToAction("index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            return View(result);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id.ToString().Equals(userId))
            {
                TempData["FailResult"] = "You cannot delete yourself";
                return View(id);
            }
            var user = await _userApiClient.GetById(id);
            
            return View(new UserDeleteRequest()
            {
                Id = id
            });
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.Delete(request.Id);
            if (result)
            {
                TempData["result"] = "Xóa người dùng thành công";
                return RedirectToAction("Index");
            }

            return View(request);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            return View(roleAssignRequest);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userApiClient.GetById(request.Id);
            bool isYourself = false;
            if (request.Id.ToString().Equals(userId))
            {
                isYourself = true;
            }
            foreach (var item in request.Roles)
            {
                if (item.Name.Equals(SystemConstant.AppSettings.AdminRole) && item.Selected == false && isYourself == true)
                {
                    TempData["FailResult"] = "Cannot unassign your admin role";
                    return View(request);
                }
            }
            var result = await _userApiClient.RoleAssign(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Assign role successfully";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = GetRoleAssignRequest(request.Id);
            return View(roleAssignRequest);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetById(id);
            var roleObj = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roleObj)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
        }
        [HttpGet]
        public async Task<IActionResult> PageActivityLog(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new UserActivityLogRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            ViewBag.keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["Result"];
            }
            var data = await _userApiClient.GetPageActivities(request);
            return View(data);
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
            var data = await _userApiClient.GetUserActivities(request);
            return View(data);
        }
        public IActionResult Forbidden()
        {
            TempData["FailResult"] = "You are not allow to access this action";
            return RedirectToAction("Index");
        }

    }
}
