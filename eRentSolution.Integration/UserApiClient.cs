using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        public async Task<ApiResult<string>> RefreshToken(UserLoginRequest login, bool isAdminPage)
        {
            var result = await PostAsync<string>($"api/users/refreshToken/{isAdminPage}", login);
            return result;
        }
        public async Task<ApiResult<string>> Delete(Guid id, string tokenName)
        {
            var result = await DeleteAsync<string>($"api/users/{id}", tokenName);
            return result;
        }
        public async Task<ApiResult<string>> UpdateAvatar(UserAvatarUpdateRequest request, string tokenName)
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
            var body = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }
        public async Task<ApiResult<string>> RegisterUser(UserRegisterRequest registerRequest)
        {
            var result = await PostAsync<string>($"/api/users", registerRequest);
            return result;
        }
        public async Task<ApiResult<string>> RoleAssign(RoleAssignRequest request, string tokenName)
        {
            var result = await PutAsync<string>($"/api/users/{request.Id}/roles", request, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> Update(UserUpdateRequest request, string tokenName)
        {
            var result = await PutAsync<string>($"/api/users/{request.Id}", request, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> UpdatePassword(UserUpdatePasswordRequest request, string tokenName)
        {
            var result = await PutAsync<string>($"/api/users/password/{request.Id}", request, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> ResetPassword(UserResetPasswordRequest request, string tokenName)
        {
            var result = await PutAsync<string>($"/api/users/ResetPassword/{request.Id}", request, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> BanUser(Guid id, string tokenName)
        {
            var result = await PutAsync<string>($"api/users/ban-user/{id}", id, tokenName);
            return result;
        }
        public async Task<ApiResult<UserViewModel>> GetById(Guid id, string tokenName)
        {
            var result = await GetPageAsync<UserViewModel>($"/api/users/{id}", tokenName);
            return result;
        }
        public async Task<ApiResult<UserViewModel>> GetUserByProductId(int productId, string tokenName)
        {
            var result = await GetAsync<UserViewModel>($"/api/users/{productId}/product", tokenName);
            return result;
        }
        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPaging(GetUserPagingRequest request, string tokenName)
        {
            var result = await GetPageAsync<PagedResult<UserViewModel>>($"/api/users/paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}", tokenName);
            return result;
        }
        public async Task<ApiResult<PagedResult<UserViewModel>>> GetStaffsPaging(GetUserPagingRequest request, string tokenName)
        {
            var result = await GetPageAsync<PagedResult<UserViewModel>>($"/api/users/staff?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}", tokenName);
            return result;
        }
        public async Task<ApiResult<PagedResult<ActivityLogViewModel>>> GetUserActivities(UserActivityLogRequest request, string tokenName)
        {
            var result = await GetPageAsync<PagedResult<ActivityLogViewModel>>($"/api/users/activity-log?id={request.Id}&pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}", tokenName);
            return result;
        }
        public async Task<ApiResult<PagedResult<ActivityLogViewModel>>> GetPageActivities(UserActivityLogRequest request, string tokenName)
        {
            var result = await GetPageAsync<PagedResult<ActivityLogViewModel>>($"/api/users/page-activity-log?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}", tokenName);
            return result;
        }
        public async Task<ApiResult<string>> ResetPasswordByEmail(UserResetPasswordByEmailRequest request, string tokenName)
        {
            var result = await PutAsync<string>($"/api/users/ResetPasswordByEmail", request, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            var result = await PostAsync<string>($"/api/users/ForgotPassword", request);
            return result;
        }
        public async Task<ApiResult<string>> SendConfirmEmail(SendConfirmEmailRequest request, string tokenName)
        {
            var result = await PutAsync<string>($"/api/users/SendConfirmEmail", request, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> ConfirmEmail(ConfirmEmailRequest request, string tokenName)
        {
            var result = await PutAsync<string>($"/api/users/ConfirmEmail", request, tokenName);
            return result;
        }
    }
}
