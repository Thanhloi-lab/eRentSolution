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
        #region ----PRODUCT-----
        public async Task<ApiResult<string>> CategoryAssign(int productId, CategoryAssignRequest request, string tokenName)
        {
            var result = await PutAsync<string>($"/api/products/{productId}/categories", request, tokenName);
            return result;
        }
        public async Task<ApiResult<int>> CreateProduct(ProductCreateRequest request, Guid userInfoId, string tokenName)
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

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Stock.ToString()) ? "" : request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.OriginalPrice.ToString()) ? "" : request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Price.ToString()) ? "" : request.Price.ToString()), "price");
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
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<int>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<int>>(body);

        }
        public async Task<ApiResult<string>> DeleteProduct(int productId, string tokenName)
        {
            var result = await DeleteAsync<string>($"/api/products/delete/{productId}", tokenName);
            return result;
        }
        public async Task<ApiResult<string>> UpdateProduct(ProductUpdateRequest request, Guid userInfoId, string tokenName)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Address) ? "" : request.Address.ToString()), "address");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoDescription) ? "" : request.SeoDescription.ToString()), "seoDescription");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoAlias) ? "" : request.SeoAlias.ToString()), "seoAlias");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoTitle) ? "" : request.SeoTitle.ToString()), "seoTitle");

            var response = await client.PutAsync($"api/products/{userInfoId}/{request.Id}", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }
        public async Task<ApiResult<string>> HideProduct(int productId, Guid userInfoId, string tokenName)
        {
            var result = await PutAsync<string>($"/api/products/hide/{userInfoId}/{productId}", productId, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> ShowProduct(int productId, Guid userInfoId, string tokenName)
        {
            var result = await PutAsync<string>($"/api/products/show/{userInfoId}/{productId}", productId, tokenName);
            return result;
        }
        public async Task<ApiResult<ProductViewModel>> GetById(int productId, string tokenName)
        {
            var result = await GetAsync<ProductViewModel>($"/api/products/{productId}", tokenName);
            return result;
        }
        public async Task<ApiResult<string>> IsMyProduct(int productId, Guid userId, string tokenName)
        {
            var result = await GetAsync<string>($"/api/products/isMyProduct/{userId}/{productId}", tokenName);
            return result;
        }
        public async Task<ApiResult<string>> CreateFeature(FeatureProductRequest request, string tokenName, Guid userInfoId)
        {
            var result = await PutAsync<string>($"/api/products/{userInfoId}/createfeature/{request.ProductId}", request, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> DeleteFeature(FeatureProductRequest request, string tokenName, Guid userInfoId)
        {
            var result = await PutAsync<string>($"/api/products/{userInfoId}/deletefeature/{request.ProductId}", request, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> ActiveProduct(int productId, Guid userInfoId, string tokenName)
        {
            var result = await PutAsync<string>($"/api/products/active/{userInfoId}/{productId}", productId, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> InActiveProduct(int productId, Guid userInfoId, string tokenName)
        {
            var result = await PutAsync<string>($"/api/products/inactive/{userInfoId}/{productId}", productId, tokenName);
            return result;
        }
        #endregion

        #region -----GET PAGE PRODUCT------
        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetPagings(GetProductPagingRequest request, string tokenName)
        {
            var result = await GetAsync<PagedResult<ProductViewModel>>(
                $"/api/products/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}" +
                $"&categoryId={request.CategoryId}" +
                $"&address={request.Address}" +
                $"&minprice={request.MinPrice}" +
                $"&maxprice={request.MaxPrice}&isGuess={request.IsGuess}" +
                $"&isStatisticMonth={request.IsStatisticMonth}" +
                $"&status={request.Status}" , tokenName);
            return result;
        }
        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetFeaturedProducts(GetProductPagingRequest request, string tokenName)
        {
            var result = await GetAsync<PagedResult<ProductViewModel>>(
                $"/api/products/feature?pageindex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}" +
                $"&categoryId={request.CategoryId}" +
                $"&address={request.Address}" +
                $"&minprice={request.MinPrice}" +
                $"&maxprice={request.MaxPrice}&isGuess={request.IsGuess}", tokenName);
            return result;
        }
        public async Task<ApiResult<List<ProductViewModel>>> GetLastestProducts(int take, string tokenName)
        {
            var result = await GetListAsync<ProductViewModel>($"/api/products/lastest/{take}", tokenName);
            return result;
        }
        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetPageProductsByUserId(GetProductPagingRequest request, Guid userId, string tokenName)
        {
            var result = await GetAsync<PagedResult<ProductViewModel>>(
                $"/api/products/{userId}/GetPageProductByUserId?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}" +
                $"&categoryId={request.CategoryId}" +
                $"&address={request.Address}" +
                $"&minprice={request.MinPrice}" +
                $"&maxprice={request.MaxPrice}&isGuess={request.IsGuess}", tokenName);
            return result;
        }
        public async Task<ApiResult<PagedResult<UserProductStatisticViewModel>>> GetStatisticUserProduct(GetProductPagingRequest request, string tokenName)
        {
            var result = await GetAsync<PagedResult<UserProductStatisticViewModel>>(
                $"/api/products/GetStatisticUserProduct?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}" +
                $"&categoryId={request.CategoryId}" +
                $"&address={request.Address}" +
                $"&minprice={request.MinPrice}" +
                $"&maxprice={request.MaxPrice}&isGuess={request.IsGuess}", tokenName);
            return result;
        }

        #endregion

        #region -----PRODUCT DETAIL------
        public async Task<ApiResult<string>> UpdateDetail(ProductDetailUpdateRequest request, Guid userId, string tokenName)
        {
            var result = await PutAsync<string>($"/api/products/updateDetail/{userId}", request, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> AddProductDetail(ProductDetailCreateRequest request, Guid userInfoId, string tokenName)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();

            if (request.Image != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "image", request.Image.FileName);
            }

            requestContent.Add(new StringContent(request.Price.ToString()==null ? "" : request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.ProductId.ToString() == null ? "" : request.ProductId.ToString()), "productId");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString() == null ? "" : request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString() == null ? "" : request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.ProductDetailName) ? "" : request.ProductDetailName), "productDetailName");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Detail) ? "" : request.Detail), "detail");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Width.ToString()) ? "" : request.Width.ToString()), "width");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Length.ToString()) ? "" : request.Length.ToString()), "length");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Detail) ? "" : request.Detail), "details");

            var response = await client.PostAsync($"api/products/addProductDetail/{userInfoId}/", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }
        public async Task<ApiResult<string>> UpdateStock(ProductUpdateStockRequest request, Guid userInfoId, string tokenName)
        {
            var result = await PutAsync<string>($"/api/products/stock/{userInfoId}", request, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> UpdatePrice(ProductUpdatePriceRequest request, Guid userInfoId, string tokenName)
        {
            var result = await PutAsync<string>($"/api/products/price/{userInfoId}", request, tokenName);
            return result;
        }
        public async Task<ApiResult<string>> AddViewcount(int productId, string tokenName)
        {
            var result = await PutAsync<string>($"/api/products/viewcount/{productId}", productId, tokenName);
            return result;
        }
        public async Task<ApiResult<ProductDetailViewModel>> GetProductDetailById(int productDetailId, string tokenName)
        {
            var result = await GetAsync<ProductDetailViewModel>($"/api/products/productDetail/{productDetailId}", tokenName);
            return result;
        }
        public async Task<ApiResult<string>> HideProductDetail(int productDetailId, Guid userInfoId, string tokenName)
        {
            throw new NotImplementedException();
        }
        public async Task<ApiResult<string>> ShowProductDetail(int productDetailId, Guid userInfoId, string tokenName)
        {
            throw new NotImplementedException();
        }
        public async Task<ApiResult<string>> DeleteDetail(int productDetailId, Guid userId, string tokenName)
        {
            var result = await DeleteAsync<string>($"/api/products/{userId}/deleteDetail/{productDetailId}", tokenName);
            return result;
        }
        #endregion

        #region ------IMAGE-------
        public async Task<ApiResult<ProductImageViewModel>> GetImageById(int imageId, string tokenName)
        {
            var result = await GetAsync<ProductImageViewModel>($"/api/products/img/{imageId}", tokenName);
            return result;
        }
        public async Task<ApiResult<string>> UpdateImage(ProductImageUpdateRequest request, string tokenName, Guid userId)
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

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.OldImageUrl) ? "" : request.OldImageUrl), "oldImageUrl");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.ImageId.ToString()) ? "" : request.ImageId.ToString()), "imageId");

            var response = await client.PutAsync($"api/products/update-img/{userId}", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            }
            else
                return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }
        public async Task<ApiResult<string>> AddImage(ProductImageCreateRequest request, string tokenName, Guid userId)
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

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Caption) ? "" : request.Caption), "caption");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.ProductDetailId.ToString()) ? "" : request.ProductDetailId.ToString()), "productDetailId");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.ProductId.ToString()) ? "" : request.ProductId.ToString()), "productId");

            var response = await client.PostAsync($"api/products/add-img/{userId}/", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            }
            else
                return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }
        public async Task<ApiResult<List<ProductImageViewModel>>> GetListImages(int productId, string tokenName)
        {
            var result = await GetListAsync<ProductImageViewModel>($"/api/products/imgs/{productId}", tokenName);
            return result;
        }
        public async Task<ApiResult<string>> DeleteImage(int imageId, Guid userId, string tokenName)
        {
            var result = await DeleteAsync<string>($"/api/products/{userId}/img/{imageId}", tokenName);
            return result;
        }

       

        #endregion
    }
}
