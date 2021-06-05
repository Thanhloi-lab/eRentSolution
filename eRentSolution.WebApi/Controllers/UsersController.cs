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
            var result = await _userService.Authenticate(request, isAdminPage);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("refreshToken/{isAdminPage}")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] UserLoginRequest request, bool isAdminPage)
        {
            var result = await _userService.RefreshToken(request, isAdminPage);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("ban-user/{id}")]
        public async Task<IActionResult> BanUser(Guid id)
        {
            var result = await _userService.BanUser(id);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetById(id);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetUserPaging([FromQuery]GetUserPagingRequest request)
        {
            var result = await _userService.GetUserPaging(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("staff")]
        public async Task<IActionResult> GetStaffsPaging([FromQuery] GetUserPagingRequest request)
        {
            var result = await _userService.GetStaffPaging(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserRegisterRequest request)
        {
            var result = await _userService.Register(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{id}/roles")]
        public async Task<IActionResult> RoleAssign(Guid id ,[FromBody]RoleAssignRequest request)
        {
            var result = await _userService.RoleAssign(id, request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            var result = await _userService.Update(id, request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("Password/{id}")]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] UserUpdatePasswordRequest request)
        {
            var result = await _userService.UpdatePassword(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("ResetPassword/{id}")]
        public async Task<IActionResult> ResetPassword(Guid id, [FromBody] UserResetPasswordRequest request)
        {
            var result = await _userService.ResetPassword(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("ResetPasswordByEmail")]
        public async Task<IActionResult> ResetPasswordByEmail([FromBody] UserResetPasswordByEmailRequest request)
        {
            var result = await _userService.ResetPasswordByEmail(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("SendConfirmEmail")]
        public async Task<IActionResult> SendEmailConfirm([FromBody] SendConfirmEmailRequest request)
        {
            var result = await _userService.SendConfirmEmail(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var result = await _userService.ConfirmEmail(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _userService.ForgotPassword(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("activity-log")]
        public async Task<IActionResult> GetUserActivity([FromQuery] UserActivityLogRequest request)
        {
            var result = await _userService.GetUserActivities(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("page-activity-log")]
        public async Task<IActionResult> GetPageActivity([FromQuery] UserActivityLogRequest request)
        {
            var result = await _userService.GetPageUserActivities(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{productId}/product")]
        public async Task<IActionResult> GetUserByProductId(int productId)
        {
            var result = await _userService.GetUserByProductId(productId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("updateAvatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage([FromForm] UserAvatarUpdateRequest request)
        {
            var result = await _userService.UpdateAvatar(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
