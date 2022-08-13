using eRentSolution.AdminApp.Models;
using eRentSolution.Data.Enums;
using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.System.Users;
using eRentSolution.ViewModels.Utilities.Contacts;
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
        private readonly IContactApiClient _contactApiClient;

        public LoginController(IUserApiClient userApiClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IContactApiClient contactApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _contactApiClient = contactApiClient;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var token = _httpContextAccessor.HttpContext.Request.Cookies[SystemConstant.AppSettings.TokenAdmin];
            //var session  = HttpContext.Session.GetString(SystemConstant.AppSettings.TokenAdmin);
            //if (!string.IsNullOrEmpty(session))
            //{
            //    token = session;
            //}
            //if (!string.IsNullOrEmpty(token))
            //{
            //    var userPrincipal = this.ValidateToken(token);
            //    if (userPrincipal == null)
            //    {
            //        Response.Cookies.Delete(SystemConstant.AppSettings.TokenAdmin);
            //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //        return RedirectToAction("index", "login");
            //    }
            //    var authProperties = new AuthenticationProperties
            //    {
            //        IsPersistent = true,
            //    };
            //    HttpContext.Session.SetString(SystemConstant.AppSettings.TokenAdmin, token);
            //    await HttpContext.SignInAsync(
            //            CookieAuthenticationDefaults.AuthenticationScheme,
            //            userPrincipal,
            //            authProperties);

            //    Response.Cookies.Append(SystemConstant.AppSettings.TokenAdmin, token, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(30) });
            //    return RedirectToAction("Index", "Home");
            //}

            //if(!string.IsNullOrEmpty(session))
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            GetContactPagingRequest request = new GetContactPagingRequest()
            {
                PageIndex = 1,
                Status = (int)(object)Status.Active,
                PageSize = 2,
            };
            var contacts = await _contactApiClient.GetAllPaging(request, SystemConstant.AppSettings.TokenAdmin);
            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["Result"];
            }
            return View(new LoginViewModel()
            {
                contacts = contacts.ResultObject.Items,
                userLoginRequest = null
            });
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            UserLoginRequest request = model.userLoginRequest;
            if (!ModelState.IsValid)
                return View(ModelState);

            var result = await _userApiClient.Authenticate(request, true);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
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
    }
}
