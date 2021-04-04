using eRentSolution.Application.System.Users;
using eRentSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate/{isAdminPage}")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]UserLoginRequest request, bool isAdminPage)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);
            var result = await _userService.Authenticate(request, isAdminPage);
            if(string.IsNullOrEmpty(result.ResultObject))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetById(id);
            if(!result.IsSuccessed)
                return BadRequest();
            return Ok(result);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetUserPaging([FromQuery]GetUserPagingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);
            var result = await _userService.GetUserPaging(request);
            return Ok(result);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);
            var result = await _userService.Register(request);
            return Ok(result);
        }
        [HttpPut("{id}/roles")]
        public async Task<IActionResult> RoleAssign(Guid id ,[FromBody]RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);
            var result = await _userService.RoleAssign(id, request);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);
            var result = await _userService.Update(id, request);
            return Ok(result);
        }

    }
}
