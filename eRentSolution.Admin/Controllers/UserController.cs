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
using System.Collections.Generic;
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
        private string userId;
        private readonly string token;
        public UserController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            ISlideApiClient slideApiClient,
            IHttpContextAccessor httpContextAccessor,
            IUserApiClient userApiClient,
            IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
            _httpContextAccessor = httpContextAccessor;
            
            token = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.TokenAdmin);
        }
        [HttpGet]
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
        [HttpGet]
        public async Task<IActionResult> Staffs(string keyword, int pageIndex = 1, int pageSize = 10)
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
            var data = await _userApiClient.GetStaffsPaging(request, SystemConstant.AppSettings.TokenAdmin);
            return View(data.ResultObject);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.SetString(SystemConstant.AppSettings.TokenAdmin, "");
            Response.Cookies.Delete(SystemConstant.AppSettings.TokenAdmin);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "login");
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleApiClient.GetAll(SystemConstant.AppSettings.TokenAdmin);
            var listRoles = new List<SelectItem>();
            foreach (var item in roles.ResultObject)
            {
                listRoles.Add(new SelectItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Selected = false
                });
            }
            return View(new UserRegisterRequest()
            {
                Roles = listRoles
            });
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
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
                    Name = target.ResultObject.FirstName,
                    OldAvatarFilePath = target.ResultObject.AvatarFilePath
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
        public async Task<IActionResult> ChangePassword(Guid id)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!id.ToString().Equals(userId))
            {
                TempData["FailResult"] = "Không thể cập nhật thông tin của người dùng khác";
                return RedirectToAction("Index");
            }
            var result = await _userApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if(result.IsSuccessed)
            {
                return View(new UserUpdatePasswordRequest()
                {
                    Id = id,
                    FirstName = result.ResultObject.FirstName
                });
            }
            return RedirectToAction("Error", "Home");
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
        public async Task<IActionResult> ResetPassword(Guid id)
        {
            var result = await _userApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if(result.IsSuccessed)
            {
                return View(new UserResetPasswordRequest()
                {
                    Id = id,
                    Token = token,
                    NewPassword = SystemConstant.AppSettings.PasswordReseted,
                    User = result.ResultObject
                });
            }
            ModelState.AddModelError("", result.Message);
            return RedirectToAction("index");
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
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index","home");
            }
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

            var result = await _userApiClient.ResetPasswordByEmail(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed == true)
            {
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> SendConfirmEmail(string email)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userApiClient.GetById(Guid.Parse(userId), SystemConstant.AppSettings.TokenAdmin);
            if(user.IsSuccessed)
            {
                if(!user.ResultObject.Email.Equals(email) || user.ResultObject.EmailConfirmed)
                {
                    return RedirectToAction("index", "home");
                }
            }
            SendConfirmEmailRequest request = new SendConfirmEmailRequest()
            {
                Email=email
            };
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> SendConfirmEmail(SendConfirmEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View(request);
            }
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            request.CurrentDomain = _configuration["CurrentDomain"];
            var result = await _userApiClient.SendConfirmEmail(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed == true)
            {
                TempData["result"] = result.ResultObject;
                return Redirect($"/user/details/{userId}");
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
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _userApiClient.ConfirmEmail(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed == true)
            {
                TempData["result"] = result.ResultObject;
                return Redirect($"/user/details/{userId}");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if(result.IsSuccessed)
            {
                if (TempData["result"] != null)
                {
                    ViewBag.success = TempData["Result"];
                }
                return View(result.ResultObject);
            }    
            TempData["FailResult"] = result.Message;
            return RedirectToAction("Index");
        }
        [Authorize(Roles = SystemConstant.AppSettings.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
                UserName = user.ResultObject.UserName,
                PhoneNumber = user.ResultObject.PhoneNumber
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
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
        public async Task<IActionResult> ActivityLog(Guid targetId, string keyword, int pageIndex = 1, int pageSize = 10)
        {
            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var request = new UserActivityLogRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Id = Guid.Parse(userId)
            };
            if(targetId!=null)
            {
                if(targetId!= new Guid("{00000000-0000-0000-0000-000000000000}"))
                    request.Id = targetId;
            }
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
