
using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eRentSolution.WebApp.Controllers.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public SideBarViewComponent(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await _categoryApiClient.GetAll(SystemConstant.AppSettings.TokenWebApp);
            return View(item.ResultObject);
        }
    }
}
