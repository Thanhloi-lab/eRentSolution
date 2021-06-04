using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.Utilities.Slides;
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
    public class SlideApiClient : BaseApiClient, ISlideApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public SlideApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<ApiResult<string>> CreateSlide(SlideCreateRequest request, string tokenName, Guid userInfoId)
        {

            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();

            if (request.ImageFile != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ImageFile.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ImageFile.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "imageFile", request.ImageFile.FileName);
            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.ProductUrl) ? "" : request.ProductUrl.ToString()), "productUrl");

            var response = await client.PostAsync($"api/slides/{userInfoId}/create/{request.ProductId}", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }
        public async Task<ApiResult<string>> DeleteSlide(SlideStatusRequest request, string tokenName, Guid userInfoId)
        {
            var result = await DeleteAsync<string>($"/api/Slides/{userInfoId}/delete/{request.Id}", tokenName);
            return result;

        }
        public async Task<ApiResult<string>> HideSlide(SlideStatusRequest request, string tokenName, Guid userInfoId)
        {
            var result = await PutAsync<string>($"/api/Slides/{userInfoId}/hide/{request.Id}", request, tokenName);
            return result;

        }
        public async Task<ApiResult<string>> ShowSlide(SlideStatusRequest request, string tokenName, Guid userInfoId)
        {
            var result = await PutAsync<string>($"/api/Slides/{userInfoId}/show/{request.Id}", request, tokenName);
            return result;

        }
        public async Task<ApiResult<string>> UpdateSlide(SlideUpdateRequest request, string tokenName, Guid userInfoId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");

            var response = await client.PutAsync($"api/slides/{userInfoId}/update/{request.Id}", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }
        public async Task<ApiResult<List<SlideViewModel>>> GetAll(string tokenName)
        {
            return await GetListAsync<SlideViewModel>("/api/Slides", tokenName);
        }
        public async Task<ApiResult<PagedResult<SlideViewModel>>> GetPagings(GetSlidePagingRequest request, string tokenName)
        {
            var result = await GetAsync<PagedResult<SlideViewModel>>(
                $"/api/Slides/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}&status={request.Status}", tokenName);
            return result;
        }
        public async Task<ApiResult<SlideViewModel>> GetById(int SlideId, string tokenName)
        {
            var result = await GetAsync<SlideViewModel>($"/api/slides/{SlideId}", tokenName);
            return result;
        }
    }
}
