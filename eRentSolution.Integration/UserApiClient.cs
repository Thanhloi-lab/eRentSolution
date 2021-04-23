using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public class UserApiClient : BaseApiClient ,IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<string>> Authenticate(UserLoginRequest login, bool isAdminPage)
        {
            var result = await PostAsync<string>($"api/users/authenticate/{isAdminPage}", login);
            return result;
        }
        public async Task<bool> Delete(Guid id, string tokenName)
        {
            var result = await DeleteAsync<bool>($"api/users/{id}", tokenName);
            return result;
        }
        public async Task<UserViewModel> GetById(Guid id, string tokenName)
        {
            var result = await GetPageAsync<UserViewModel>($"/api/users/{id}", tokenName);
            return result.ResultObject;
        }
        public async Task<PagedResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request, string tokenName)
        {
            var result = await GetPageAsync<PagedResult<UserViewModel>>($"/api/users/paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}", tokenName);
            return result.ResultObject;
        }
        public async Task<ApiResult<bool>> RegisterUser(UserRegisterRequest registerRequest)
        {
            var result = await PostAsync<bool>($"/api/users", registerRequest);
            return result;
        }
        public async Task<ApiResult<bool>> RoleAssign(RoleAssignRequest request, string tokenName)
        {
            var result = await PutAsync<bool>($"/api/users/{request.Id}/roles", request, tokenName);
            return result;
        }
        public async Task<ApiResult<bool>> Update(UserUpdateRequest request, string tokenName)
        {
            var result = await PutAsync<bool>($"/api/users/{request.Id}", request, tokenName);
            return result;
        }
        public async Task<ApiResult<bool>> UpdatePassword(UserUpdatePasswordRequest request, string tokenName)
        {
            var result = await PutAsync<bool>($"/api/users/password/{request.Id}", request, tokenName);
            return result;
        }
        public async Task<ApiResult<bool>> ResetPassword(UserResetPasswordRequest request, string tokenName)
        {
            var result = await PutAsync<bool>($"/api/users/ResetPassword/{request.Id}", request, tokenName);
            return result;
        }
        public async Task<PagedResult<ActivityLogViewModel>> GetUserActivities(UserActivityLogRequest request, string tokenName)
        {
            var result = await GetPageAsync<PagedResult<ActivityLogViewModel>>($"/api/users/activity-log?id={request.Id}&pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}", tokenName);
            return result.ResultObject;
        }
        public async Task<PagedResult<ActivityLogViewModel>> GetPageActivities(UserActivityLogRequest request, string tokenName)
        {
            var result = await GetPageAsync<PagedResult<ActivityLogViewModel>>($"/api/users/page-activity-log?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}", tokenName);
            return result.ResultObject;
        }
        public async Task<bool> UpdateAvatar(UserAvatarUpdateRequest request, string tokenName)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();

            if (request.AvatarFile != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.AvatarFile.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.AvatarFile.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "avatarFile", request.AvatarFile.FileName);
            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Id.ToString()) ? "" : request.Id.ToString()), "id");

            var result = await client.PutAsync($"/api/users/UpdateAvatar", requestContent);
            return result.IsSuccessStatusCode;
        }

        public async Task<UserViewModel> GetUserByProductId(int productId, string tokenName)
        {
            var result = await GetAsync<UserViewModel>($"/api/users/{productId}/product", tokenName);
            return result;
        }
    }
}
