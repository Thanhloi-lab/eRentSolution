using eRentSolution.Integration;
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
            token = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.TokenAdmin);
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
            var data = await _userApiClient.GetUsersPaging(request, SystemConstant.AppSettings.TokenAdmin);
            return View(data.ResultObject);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete(SystemConstant.AppSettings.TokenAdmin);
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
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View(request);
            }    
                
            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (!id.ToString().Equals(userId))
            {
                TempData["FailResult"] = "Không thể cập nhật thông tin của người dùng khác";
                return RedirectToAction("Index");
            }
            var target = await _userApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (target.IsSuccessed)
            {
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = target.ResultObject.Dob,
                    Email = target.ResultObject.Email,
                    FirstName = target.ResultObject.FirstName,
                    LastName = target.ResultObject.LastName,
                    PhoneNumber = target.ResultObject.PhoneNumber,
                    Id = id,
                    UserName = target.ResultObject.UserName
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View(request);
            }    
               
            var result = await _userApiClient.Update(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> EditAvatar(Guid id)
        {
            if (!id.ToString().Equals(userId))
            {
                TempData["FailResult"] = "Không thể cập nhật người dùng khác";
                return View(id);
            }
            var target = await _userApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
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
                
            var result = await _userApiClient.UpdateAvatar(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
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
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View(request);
            }
            var result = await _userApiClient.UpdatePassword(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
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
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View();
            }
            var result = await _userApiClient.ResetPassword(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if(result.IsSuccessed)
                return View(result);

            TempData["FailResult"] = result.Message;
            return RedirectToAction("Index");
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id.ToString().Equals(userId))
            {
                TempData["FailResult"] = "Không thể xóa tài khoản đăng nhập hiện tại";
                return RedirectToAction("Index");
            }
            var user = await _userApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if(!user.IsSuccessed)
            {
                TempData["FailResult"] = user.Message;
                return RedirectToAction("Index");
            }    
            return View(new UserDeleteRequest()
            {
                Id = id,
                Avatar = user.ResultObject.AvatarFilePath,
                Email = user.ResultObject.Email,
                FirstName = user.ResultObject.FirstName,
                LastName = user.ResultObject.LastName,
                UserName = user.ResultObject.UserName
            });
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View();
            }

            var result = await _userApiClient.BanUser(request.Id, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            if(roleAssignRequest ==null)
            {
                TempData["FailResult"] = "Không thể thực hiện thao tác, vui lòng thử lại sau";
                return RedirectToAction("Index");
            }
            return View(roleAssignRequest);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View();
            }

            //var user = await _userApiClient.GetById(request.Id, SystemConstant.AppSettings.TokenAdmin);
            bool isYourself = false;
            if (request.Id.ToString().Equals(userId))
            {
                isYourself = true;
            }
            foreach (var item in request.Roles)
            {
                if (item.Name.Equals(SystemConstant.AppSettings.AdminRole) && item.Selected == false && isYourself == true)
                {
                    ModelState.AddModelError("", "Không thể xóa quyền quản trị của bản thân");
                    return View(request);
                }
            }
            
            var result = await _userApiClient.RoleAssign(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = GetRoleAssignRequest(request.Id);
            if (roleAssignRequest == null)
            {
                TempData["FailResult"] = "Không thể thực hiện thao tác, vui lòng thử lại sau";
                return RedirectToAction("Index");
            }
            return View(roleAssignRequest);
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var user = await _userApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if(!user.IsSuccessed)
                return null;
            var roles = await _roleApiClient.GetAll(SystemConstant.AppSettings.TokenAdmin);
            if (!roles.IsSuccessed)
                return null;

            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roles.ResultObject)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = user.ResultObject.Roles.Contains(role.Name)
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
            var data = await _userApiClient.GetPageActivities(request, SystemConstant.AppSettings.TokenAdmin);
            return View(data.ResultObject);
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
            var data = await _userApiClient.GetUserActivities(request, SystemConstant.AppSettings.TokenAdmin);
            return View(data.ResultObject);
        }
        public IActionResult Forbidden()
        {
            TempData["FailResult"] = "Bạn không có quyền để thực hiện thao tác này";
            return RedirectToAction("Index");
        }

    }
}
