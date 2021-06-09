using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    public class BaseController : Controller
    {

        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserApiClient _userApiClient;

        public BaseController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            ISlideApiClient slideApiClient,
            IHttpContextAccessor httpContextAccessor,
            IUserApiClient userApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
            _slideApiClient = slideApiClient;
            _httpContextAccessor = httpContextAccessor;
            _userApiClient = userApiClient;
        }

        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = HttpContext.Session.GetString(SystemConstant.AppSettings.TokenAdmin);
            var cookies = _httpContextAccessor.HttpContext.Request.Cookies[SystemConstant.AppSettings.TokenAdmin];

            if (string.IsNullOrEmpty(cookies) && string.IsNullOrEmpty(session))
            {
                if (User.Identity.IsAuthenticated)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Result = new RedirectToActionResult("Index", "Login", null);
                    base.OnActionExecuting(context);
                }

            }
            if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(cookies))
            {
                try
                {
                    await RefreshToken();
                }
                catch (Exception e)
                {

                }
            }

            try
            {
                session = HttpContext.Session.GetString(SystemConstant.AppSettings.TokenAdmin);
            }
            catch (Exception e)
            {
                session = "";
            }
            
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
        public async Task RefreshToken()
        {
            UserLoginRequest request = new UserLoginRequest()
            {
                RememberMe = bool.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.IsPersistent).Value),
                UserName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.GivenName).Value,
                Password = "1"
            };

            var result = await _userApiClient.RefreshToken(request, true);
            if (!result.IsSuccessed)
            {
                Response.Cookies.Delete(SystemConstant.AppSettings.TokenAdmin);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return;
            }

            var userPrincipal = this.ValidateToken(result.ResultObject);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = request.RememberMe,
                ExpiresUtc = DateTimeOffset.Now.AddDays(30)
            };
            try
            {
                HttpContext.Session.SetString(SystemConstant.AppSettings.TokenAdmin, result.ResultObject);
            }
            catch(Exception e)
            {

            }
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            if (request.RememberMe)
                Response.Cookies.Append(SystemConstant.AppSettings.TokenAdmin, result.ResultObject, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(30) });
        }

        public async override void OnActionExecuted(ActionExecutedContext context)
        {
            string session;
            try
            {
                session = HttpContext.Session.GetString(SystemConstant.AppSettings.TokenAdmin);
            }
            catch (Exception e)
            {
                session = "";
            }
            if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(session))
            {
                try
                {
                    await RefreshToken();
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
