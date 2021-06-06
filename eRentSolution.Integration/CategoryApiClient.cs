using eRentSolution.ViewModels.Catalog.Categories;
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
    public class CategoryApiClient : BaseApiClient ,ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CategoryApiClient(IHttpClientFactory httpClientFactory, 
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<List<CategoryViewModel>>> GetAll(string tokenName)
        {
            var result = await GetListAsync<CategoryViewModel>($"api/categories", tokenName);
            return result;
        }
        public async Task<ApiResult<List<CategoryViewModel>>> GetAllCategoryByProductId(int productId, string tokenName)
        {
            var result = await GetListAsync<CategoryViewModel>($"api/categories/productcategories/{productId}", tokenName);
            return result;
        }
        public async Task<ApiResult<CategoryViewModel>> GetById(int id, string tokenName)
        {
            var result = await GetAsync<CategoryViewModel>($"api/categories/{id}", tokenName);
            return result;
        }
        public async Task<ApiResult<PagedResult<CategoryViewModel>>> GetPagings(GetCategoryPagingRequest request, string tokenName)
        {
            var result = await GetAsync<PagedResult<CategoryViewModel>>(
                $"/api/categories/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}", tokenName);
            return result;
        }
        public async Task<ApiResult<string>> UpdateImage(CategoryImageUpdateRequest request, string tokenName)
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
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.CategoryId.ToString()) ? "" : request.CategoryId.ToString()), "categoryId");

            var result = await client.PutAsync($"/api/categories/UpdateImage", requestContent);
            var body = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }
        public async Task<ApiResult<string>> CreateCategory(CategoryCreateRequest request, string tokenName)
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
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.ParentId.ToString()) ? "" : request.ParentId.ToString()), "parentId");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.CategoryName.ToString()) ? "" : request.CategoryName.ToString()), "categoryName");

            var result = await client.PostAsync($"/api/categories/createCategory", requestContent);
            var body = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }
        public async Task<ApiResult<string>> UpdateCategory(CategoryUpdateRequest request, string tokenName)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.CategoryName.ToString()) ? "" : request.CategoryName.ToString()), "categoryName");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.CategoryId.ToString()) ? "" : request.CategoryId.ToString()), "categoryId");

            var result = await client.PutAsync($"/api/categories/updateCategory", requestContent);
            var body = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }
        public async Task<ApiResult<string>> DeteleCategory(CategoryStatusRequest request, string tokenName)
        {
            var result = await DeleteAsync<string>($"/api/categories/deleteCategory/{request.Id}", tokenName);
            return result;
        }
    }
}
