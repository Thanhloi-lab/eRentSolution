using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.Utilities.Slides;
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

        public async Task<bool> CreateSlide(SlideCreateRequest request, string tokenName, Guid userInfoId)
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

            var response = await client.PostAsync($"api/slides/{userInfoId}/create/{request.ProductId}", requestContent);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteSlide(SlideStatusRequest request, string tokenName, Guid userInfoId)
        {
            var result = await DeleteAsync<bool>($"/api/Slides/{userInfoId}/delete/{request.Id}", tokenName);
            return result;

        }
        public async Task<bool> HideSlide(SlideStatusRequest request, string tokenName, Guid userInfoId)
        {
            var result = await PutAsync<bool>($"/api/Slides/{userInfoId}/hide/{request.Id}", request, tokenName);
            return result.ResultObject;

        }
        public async Task<bool> ShowSlide(SlideStatusRequest request, string tokenName, Guid userInfoId)
        {
            var result = await PutAsync<bool>($"/api/Slides/{userInfoId}/show/{request.Id}", request, tokenName);
            return result.ResultObject;

        }
        public async Task<bool> UpdateSlide(SlideUpdateRequest request, string tokenName, Guid userInfoId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");

            var response = await client.PutAsync($"api/slides/{userInfoId}/update/{request.Id}", requestContent);
            return response.IsSuccessStatusCode;
        }
        public async Task<List<SlideViewModel>> GetAll(string tokenName)
        {
            return await GetListAsync<SlideViewModel>("/api/Slides", tokenName);
        }
        public async Task<PagedResult<SlideViewModel>> GetPagings(GetSlidePagingRequest request, string tokenName)
        {
            var result = await GetAsync<PagedResult<SlideViewModel>>(
                $"/api/Slides/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}&status={request.Status}", tokenName);
            return result;
        }
        public async Task<SlideViewModel> GetById(int SlideId, string tokenName)
        {
            var result = await GetAsync<SlideViewModel>($"/api/slides/{SlideId}", tokenName);
            return result;
        }
    }
}
