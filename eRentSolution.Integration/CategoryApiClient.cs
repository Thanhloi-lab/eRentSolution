using eRentSolution.ViewModels.Catalog.Categories;
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

        public async Task<List<CategoryViewModel>> GetAll(string tokenName)
        {
            var result = await GetListAsync<CategoryViewModel>($"api/categories", tokenName);
            return result;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryByProductId(int productId, string tokenName)
        {
            var result = await GetListAsync<CategoryViewModel>($"api/categories/productcategories/{productId}", tokenName);
            return result;
        }

        public async Task<CategoryViewModel> GetById(int id, string tokenName)
        {
            var result = await GetAsync<CategoryViewModel>($"api/categories/{id}", tokenName);
            return result;
        }
        public async Task<bool> UpdateImage(CategoryImageUpdateRequest request, string tokenName)
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
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.CategoryId.ToString()) ? "" : request.CategoryId.ToString()), "id");

            var result = await client.PutAsync($"/api/users/UpdateAvatar", requestContent);
            return result.IsSuccessStatusCode;
        }
    }
}
