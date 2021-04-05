using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public class UserApiClient : BaseApiClient ,IUserApiClient
    {
        public UserApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<string>> Authenticate(UserLoginRequest login, bool isAdminPage)
        {
            var result = await PostAsync<string>($"api/users/authenticate/{isAdminPage}", login);
            return result;
        }
        public async Task<bool> Delete(Guid id)
        {
            var result = await DeleteAsync<bool>($"api/users/{id}");
            return result;
        }
        public async Task<UserViewModel> GetById(Guid id)
        {
            var result = await GetPageAsync<UserViewModel>($"/api/users/{id}");
            return result.ResultObject;
        }
        public async Task<PagedResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request)
        {
            var result = await GetPageAsync<PagedResult<UserViewModel>>($"/api/users/paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
            return result.ResultObject;
        }
        public async Task<ApiResult<bool>> RegisterUser(UserRegisterRequest registerRequest)
        {
            var result = await PostAsync<bool>($"/api/users", registerRequest);
            return result;
        }
        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var result = await PutAsync<bool>($"/api/users/{id}/roles", request);
            return result;
        }
        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request)
        {
            var result = await PutAsync<bool>($"/api/users/{id}", request);
            return result;
        }
        public async Task<ApiResult<bool>> UpdatePassword(UserUpdatePasswordRequest request)
        {
            var result = await PutAsync<bool>($"/api/users/password/{request.Id}", request);
            return result;
        }
        public async Task<ApiResult<bool>> ResetPassword(UserResetPasswordRequest request)
        {
            var result = await PutAsync<bool>($"/api/users/ResetPassword/{request.Id}", request);
            return result;
        }
        public async Task<PagedResult<ActivityLogViewModel>> GetUserActivities(UserActivityLogRequest request)
        {
            var result = await GetPageAsync<PagedResult<ActivityLogViewModel>>($"/api/users/activity-log?id={request.Id}&pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
            return result.ResultObject;
        }
        public async Task<PagedResult<ActivityLogViewModel>> GetPageActivities(UserActivityLogRequest request)
        {
            var result = await GetPageAsync<PagedResult<ActivityLogViewModel>>($"/api/users/page-activity-log?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
            return result.ResultObject;
        }
    }
}
