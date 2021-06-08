using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IUserApiClient userApiClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies[SystemConstant.AppSettings.TokenWebApp];
            var session = HttpContext.Session.GetString(SystemConstant.AppSettings.TokenWebApp);
            if(!string.IsNullOrEmpty(session))
            {
                token = session;
            }
            if (!string.IsNullOrEmpty(token))
            {
                var userPrincipal = this.ValidateToken(token);
                if (userPrincipal == null)
                {
                    Response.Cookies.Delete(SystemConstant.AppSettings.TokenWebApp);
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return View();
                }
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };
                HttpContext.Session.SetString(SystemConstant.AppSettings.TokenWebApp, token);
                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

                Response.Cookies.Append(SystemConstant.AppSettings.TokenWebApp, token, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(30) });
                return RedirectToAction("Index", "Home");
            }
            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["Result"];
            }
            if (!string.IsNullOrEmpty(session))
            {
                return RedirectToAction("Index", "Home");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Đăng nhập không hợp lệ");
                return View(request);
            }    
                
            var result = await _userApiClient.Authenticate(request, false);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }

            var userPrincipal = this.ValidateToken(result.ResultObject);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = request.RememberMe,
                ExpiresUtc = DateTimeOffset.Now.AddDays(30)
            };
            HttpContext.Session.SetString(SystemConstant.AppSettings.TokenWebApp, result.ResultObject);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            
            if (request.RememberMe)
                Response.Cookies.Append(SystemConstant.AppSettings.TokenWebApp, result.ResultObject, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(30) });
            return RedirectToAction("Index", "Home");
        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = false;

            validationParameters.ValidAudience = _configuration["Tokens:Audience"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            ClaimsPrincipal principal = null;
            try
            {
                principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            }
            catch (Exception e)
            {
                return null;
            }
            return principal;
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.SetString(SystemConstant.AppSettings.TokenWebApp, "");
            Response.Cookies.Delete(SystemConstant.AppSettings.TokenWebApp);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View(request);
            }    
                
            var result = await _userApiClient.RegisterUser(request);
            if (!result.IsSuccessed)
            {
                TempData["failResult"] = result.Message;
                return View(request);
            }

            var loginResult = await _userApiClient.Authenticate(new UserLoginRequest()
            {
                UserName = request.UserName,
                Password = request.Password,
                RememberMe = false
            }, false);

            var userPrincipal = this.ValidateToken(loginResult.ResultObject);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                IsPersistent = false
            };
            HttpContext.Session.SetString(SystemConstant.AppSettings.TokenWebApp, loginResult.ResultObject);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            return RedirectToAction("Index", "Home");
        }
    }
}
