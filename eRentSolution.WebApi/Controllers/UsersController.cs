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
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]UserLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);
            var result = await _userService.Authenticate(request);
            if(string.IsNullOrEmpty(result.ResultObject))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _userService.Delete(id);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _userService.GetById(id);
            return Ok(result);
        }
        [HttpGet("paging")]
        public IActionResult GetUserPaging([FromQuery]GetUserPagingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);
            var result = _userService.GetUserPaging(request);
            return Ok(result);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register([FromBody]UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);
            var result = _userService.Register(request);
            return Ok(result);
        }
        [HttpPut("{id}/roles")]
        public IActionResult RoleAssign(Guid id ,[FromBody]RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);
            var result = _userService.RoleAssign(id, request);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);
            var result = _userService.Update(id, request);
            return Ok(result);
        }

    }
}
