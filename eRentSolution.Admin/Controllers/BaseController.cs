﻿using eRentSolution.Utilities.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eRentSolution.AdminApp.Controllers
{
    public class BaseController : Controller
    {
        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var session = HttpContext.Session.GetString(SystemConstant.AppSettings.TokenAdmin);
            var cookies = httpContextAccessor.HttpContext.Request.Cookies[CookieAuthenticationDefaults.AuthenticationScheme];
            if (string.IsNullOrEmpty(cookies) && string.IsNullOrEmpty(session) && User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
