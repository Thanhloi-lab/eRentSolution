using eRentSolution.Application.Utilities.Contacts;
using eRentSolution.ViewModels.Utilities.Contacts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRentSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetContactPagingRequest request)
        {
            var result = await _contactService.GetAllPaging(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _contactService.GetAll();
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetById(int contactId)
        {
            var result = await _contactService.GetById(contactId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ContactCreateRequest request)
        {
            var result = await _contactService.AddContact(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ContactUpdateRequest request)
        {
            var result = await _contactService.UpdateContact(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("delete/{contactId}")]
        public async Task<IActionResult> Delete(int contactId)
        {
            ContactStatusRequest request = new ContactStatusRequest()
            {
                Id = contactId,
            };
            var result = await _contactService.DeleteContact(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("hide")]
        public async Task<IActionResult> Hide([FromBody] ContactStatusRequest request)
        {
            var result = await _contactService.HideContact(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("show")]
        public async Task<IActionResult> Show([FromBody] ContactStatusRequest request)
        {
            var result = await _contactService.ShowContact(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
