using eRentSolution.Integration;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Utilities.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRentSolution.AdminApp.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        public readonly IContactApiClient _contactApiClient;

        public ContactController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient,
            ISlideApiClient slideApiClient,
            IHttpContextAccessor httpContextAccessor,
            IUserApiClient userApiClient,
            IContactApiClient contactApiClient)
        {
            _contactApiClient = contactApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int? status, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetContactPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Status = status,
            };

            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }

            var contacts = await _contactApiClient.GetAllPaging(request, SystemConstant.AppSettings.TokenAdmin);
            ViewBag.Keyword = keyword;

            var viewBagStatus = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Hoạt động",
                    Value = "1",
                    Selected = status.HasValue && status.Value.ToString() == "1"
                },
                new SelectListItem()
                {
                    Text = "Không Hoạt động",
                    Value = "0",
                    Selected = status.HasValue && status.Value.ToString() == "0"
                }
            };

            ViewBag.Status = viewBagStatus;
            return View(contacts.ResultObject);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _contactApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (!contact.IsSuccessed)
            {
                TempData["failResult"] = contact.Message;
                return RedirectToAction("index");
            }
            var contactViewModel = new ContactUpdateRequest()
            {
                Id = id,
                Name = contact.ResultObject.Name,
                Email = contact.ResultObject.Email,
                Message = contact.ResultObject.Message,
                PhoneNumber = contact.ResultObject.PhoneNumber
            };
            return View(contactViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] ContactUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _contactApiClient.UpdateContact(request, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new ContactStatusRequest()
            {
                Id = id,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ContactStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _contactApiClient.DeleteContact(request, SystemConstant.AppSettings.TokenAdmin);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request.Id);
        }
        [HttpGet]
        public IActionResult Hide(int id)
        {
            return View(new ContactStatusRequest()
            {
                Id = id,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Hide(ContactStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _contactApiClient.HideContact(request, SystemConstant.AppSettings.TokenAdmin);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request.Id);
        }
        [HttpGet]
        public IActionResult Show(int id)
        {
            return View(new ContactStatusRequest()
            {
                Id = id,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Show(ContactStatusRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _contactApiClient.ShowContact(request, SystemConstant.AppSettings.TokenAdmin);

            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request.Id);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _contactApiClient.GetById(id, SystemConstant.AppSettings.TokenAdmin);
            if (result.IsSuccessed)
                return View(result.ResultObject);
            TempData["failResult"] = result.Message;
            return RedirectToAction("intdex");
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ContactCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _contactApiClient.AddContact(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObject;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
    }
}
