using eRentSolution.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRentSolution.AdminApp.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session.GetString(SystemConstant.AppSettings.TokenAdmin);
            if(session == null)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
