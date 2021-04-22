using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.ProductDetails;
using eRentSolution.ViewModels.Catalog.ProductImages;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
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
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int productId, CategoryAssignRequest request, string tokenName)
        {
            var result = await PutAsync<bool>($"/api/products/{productId}/categories", request, tokenName);
            return result;
        }
        public async Task<bool> CreateProduct(ProductCreateRequest request, Guid userInfoId, string tokenName)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();

            if(request.ThumbnailImage!=null)
            {
                byte[] data;
                using(var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Detail) ? "" : request.Detail), "detail");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Width.ToString()) ? "" : request.Width.ToString()), "width");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Length.ToString()) ? "" : request.Length.ToString()), "length");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Address) ? "" : request.Address), "address");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description), "description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Detail) ? "" : request.Detail), "details");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoDescription) ? "" : request.SeoDescription), "seoDescription");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoAlias) ? "" : request.SeoAlias), "seoAlias");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoTitle) ? "" : request.SeoTitle), "seoTitle");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SubProductName) ? "" : request.SubProductName), "subProductName");

            var response = await client.PostAsync($"api/products/{userInfoId}", requestContent);
            return response.IsSuccessStatusCode;

        }
        public async Task<bool> DeleteProduct(int productId, string tokenName)
        {
            var result = await DeleteAsync<bool>($"/api/products/delete/{productId}", tokenName);
            return result;
        }
        public async Task<bool> HideProduct(int productId, Guid userInfoId, string tokenName)
        {
            var result = await DeleteAsync<bool>($"/api/products/hide/{userInfoId}/{productId}", tokenName);
            return result;
        }
        public async Task<bool> ShowProduct(int productId, Guid userInfoId, string tokenName)
        {
            var result = await DeleteAsync<bool>($"/api/products/show/{userInfoId}/{productId}", tokenName);
            return result;
        }
        public async Task<ProductViewModel> GetById(int productId, string tokenName)
        {
            var result = await GetAsync<ProductViewModel>($"/api/products/{productId}", tokenName);
            return result;
        }
        public async Task<PagedResult<ProductViewModel>> GetFeaturedProducts(GetProductPagingRequest request, string tokenName)
        {
            var result = await GetAsync<PagedResult<ProductViewModel>>($"/api/products/feature?pageindex={request.PageIndex}" +
                $"&pagesize={request.PageSize}&keyword={request.Keyword}&categoryId={request.CategoryId}", tokenName);
            return result;
        }
        public async Task<List<ProductViewModel>> GetLastestProducts(int take, string tokenName)
        {
            var result = await GetListAsync<ProductViewModel>($"/api/products/lastest/{take}", tokenName);
            return result;
        }
        public async Task<PagedResult<ProductViewModel>> GetPagings(GetProductPagingRequest request, string tokenName)
        {
            var result = await GetAsync<PagedResult<ProductViewModel>>(
                $"/api/products/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}" +
                $"&categoryId={request.CategoryId}", tokenName);
            return result;
        }
        public async Task<bool> UpdateProduct(ProductUpdateRequest request, Guid userInfoId, string tokenName)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();

            //if (request.ThumbnailImage != null)
            //{
            //    byte[] data;
            //    using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
            //    {
            //        data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
            //    }
            //    ByteArrayContent bytes = new ByteArrayContent(data);
            //    requestContent.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            //}

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Address) ? "" : request.Address.ToString()), "address");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.IsFeatured.ToString()) ? "" : request.IsFeatured.ToString()), "isFeatured");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");
            //requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Details) ? "" : request.Details.ToString()), "details");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoDescription) ? "" : request.SeoDescription.ToString()), "seoDescription");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoAlias) ? "" : request.SeoAlias.ToString()), "seoAlias");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoTitle) ? "" : request.SeoTitle.ToString()), "seoTitle");

            var response = await client.PutAsync($"api/products/{userInfoId}/{request.Id}", requestContent);
            return response.IsSuccessStatusCode;
        }
        public async Task<List<ProductImageViewModel>> GetListImages(int productId, string tokenName)
        {
            var result = await GetListAsync<ProductImageViewModel>($"/api/products/imgs/{productId}", tokenName);
            return result;
        }
        public async Task<bool> CreateFeature(FeatureProductRequest request, string tokenName, Guid userInfoId)
        {
            //var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            //var client = _httpClientFactory.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            //client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //var json = JsonConvert.SerializeObject(request);
            //var httpContext = new StringContent(json, Encoding.UTF8, "application/json");

            //var respond = await client.PutAsync($"/api/products/{userInfoId}/createfeature/{request.ProductId}", httpContext);
            //var body = await respond.Content.ReadAsStringAsync();
            //if (respond.IsSuccessStatusCode)
            //    return true;
            //return false;
            var result = await PutAsync<bool>($"/api/products/{userInfoId}/createfeature/{request.ProductId}", request, tokenName);
            return result.ResultObject;
        }
        public async Task<bool> DeleteFeature(FeatureProductRequest request, string tokenName, Guid userInfoId)
        {
            var result = await PutAsync<bool>($"/api/products/{userInfoId}/deletefeature/{request.ProductId}", request, tokenName);
            return result.ResultObject;
        }
        public async Task<PagedResult<ProductViewModel>> GetPageProductsByUserId(GetProductPagingRequest request, Guid userId, string tokenName)
        {
            var result = await GetAsync<PagedResult<ProductViewModel>>(
                $"/api/products/{userId}/GetPageProductByUserId?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}" +
                $"&categoryId={request.CategoryId}", tokenName);
            return result;
        }
        public async Task<bool> UpdateDetail(ProductDetailUpdateRequest request, Guid userId, string tokenName)
        {
            var result = await DeleteAsync<bool>($"/api/products/updateDetail/{userId}/{request.Id}", tokenName);
            return result;
        }
        public async Task<bool> IsMyProduct(int productId, Guid userId, string tokenName)
        {
            var result = await GetAsync<bool>($"/api/products/isMyProduct/{userId}/{productId}", tokenName);
            return result;
        }

    }
}
