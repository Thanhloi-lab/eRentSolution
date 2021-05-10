using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eRentSolution.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using eRentSolution.AdminApp.Controllers;

namespace eRentSolution.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            //var token = _httpContextAccessor.HttpContext.Request.Cookies[SystemConstant.AppSettings.TokenAdmin];
            //var session = HttpContext.Session.GetString(SystemConstant.AppSettings.TokenAdmin);
            //if (string.IsNullOrEmpty(token) && string.IsNullOrEmpty(session))
            //    return RedirectToAction("Index", "login");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
