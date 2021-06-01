﻿using eRentSolution.Integration;
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.AdminApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(IUserApiClient userApiClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies[SystemConstant.AppSettings.TokenAdmin];
            if (!string.IsNullOrEmpty(token))
            {
                var userPrincipal = this.ValidateToken(token);
                if (userPrincipal == null)
                {
                    Response.Cookies.Delete(SystemConstant.AppSettings.TokenAdmin);
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("index", "login");
                }
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };
                HttpContext.Session.SetString(SystemConstant.AppSettings.TokenAdmin, token);
                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

                Response.Cookies.Append(SystemConstant.AppSettings.TokenAdmin, token, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(30) });
                return RedirectToAction("Index", "Home");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserLoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);

            var result = await _userApiClient.Authenticate(request, true);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }

            var userPrincipal = this.ValidateToken(result.ResultObject);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = request.RememberMe,
                ExpiresUtc = DateTimeOffset.Now.AddDays(30)
            };
            HttpContext.Session.SetString(SystemConstant.AppSettings.TokenAdmin, result.ResultObject);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            if(request.RememberMe)
                Response.Cookies.Append(SystemConstant.AppSettings.TokenAdmin, result.ResultObject, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(30) });
            return RedirectToAction("Index", "Home");
        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

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
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            UserLoginRequest request = new UserLoginRequest()
            {
                RememberMe = true,//bool.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.IsPersistent).Value),
                UserName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.GivenName).Value,
                Password = "1"
            };

            var result = await _userApiClient.RefreshToken(request, true);
            if (!result.IsSuccessed)
            {
                Response.Cookies.Delete(SystemConstant.AppSettings.TokenAdmin);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return BadRequest();
            }

            var userPrincipal = this.ValidateToken(result.ResultObject);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = request.RememberMe,
                ExpiresUtc = DateTimeOffset.Now.AddDays(30)
            };
            HttpContext.Session.SetString(SystemConstant.AppSettings.TokenAdmin, result.ResultObject);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            if (request.RememberMe)
                Response.Cookies.Append(SystemConstant.AppSettings.TokenAdmin, result.ResultObject, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(30) });
            return Ok();
        }
    }
}
