using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public class BaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseApiClient(IHttpClientFactory httpClientFactory
            , IConfiguration configuration
            , IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async Task<ApiResult<List<T>>> GetListAsync<T>(string url, string tokenName)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var respond = await client.GetAsync(url);
            var body = await respond.Content.ReadAsStringAsync();
            if (respond.IsSuccessStatusCode)
            {
                ApiSuccessResult<List<T>> myDeserializedObjList = (ApiSuccessResult<List<T>>)JsonConvert.DeserializeObject(body, typeof(ApiSuccessResult<List<T>>));
                return myDeserializedObjList;
            }
            ApiErrorResult<List<T>> error = (ApiErrorResult<List<T>>)JsonConvert.DeserializeObject(body, typeof(ApiErrorResult<List<T>>));
            return error;

        }
        protected async Task<ApiResult<T>> GetAsync<T>(string url, string tokenName)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var respond = await client.GetAsync(url);
            var body = await respond.Content.ReadAsStringAsync();
            if (respond.IsSuccessStatusCode)
            {
                ApiSuccessResult<T> myDeserializedObjList = (ApiSuccessResult<T>)JsonConvert.DeserializeObject(body, typeof(ApiSuccessResult<T>));
                return myDeserializedObjList;
            }
            ApiErrorResult<T> error = (ApiErrorResult<T>)JsonConvert.DeserializeObject(body, typeof(ApiErrorResult<T>));
            return error;
        }
        protected async Task<ApiResult<T>> GetPageAsync<T>(string url, string tokenName)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var respond = await client.GetAsync(url);
            var body = await respond.Content.ReadAsStringAsync();
            if (respond.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<ApiSuccessResult<T>>(body);
                return result;
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<T>>(body);
        }
        protected async Task<ApiResult<T>> DeleteAsync<T>(string url, string tokenName)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var respond = await client.DeleteAsync(url);
            var body = await respond.Content.ReadAsStringAsync();
            if (respond.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<ApiSuccessResult<T>>(body);
                return result;
            }
            var error = JsonConvert.DeserializeObject<ApiErrorResult<T>>(body);
            return error;
        }
        protected async Task<ApiResult<T>> PostAsync<T>(string url, object obj)
        {
            //var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(obj);
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");

            var respond = await client.PostAsync(url, httpContext);
            var body = await respond.Content.ReadAsStringAsync();
            if (respond.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<T>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<T>>(body);
        }
        protected async Task<ApiResult<T>> PutAsync<T>(string url, object obj, string tokenName)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(obj);
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");

            var respond = await client.PutAsync(url, httpContext);
            var body = await respond.Content.ReadAsStringAsync();
            if (respond.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<T>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<T>>(body);
        }
    }
}
