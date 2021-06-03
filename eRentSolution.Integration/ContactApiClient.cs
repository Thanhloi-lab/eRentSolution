using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.Utilities.Contacts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public class ContactApiClient : BaseApiClient, IContactApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ContactApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<string>> AddContact(ContactCreateRequest request)
        {
            //var session = _httpContextAccessor.HttpContext.Session.GetString(tokenName);
            //var client = _httpClientFactory.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            //client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //var requestContent = new MultipartFormDataContent();

            //requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
            //requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Email) ? "" : request.Email.ToString()), "email");
            //requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Message) ? "" : request.Message.ToString()), "message");
            //requestContent.Add(new StringContent(string.IsNullOrEmpty(request.PhoneNumber) ? "" : request.PhoneNumber.ToString()), "phoneNumber");

            //var response = await client.PostAsync($"api/contacts/create", requestContent);
            //var body = await response.Content.ReadAsStringAsync();
            //if (response.IsSuccessStatusCode)
            //{
            //    return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
            //}
            //return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
            var result = await PostAsync<string>($"/api/contacts/create", request);
            return result;
        }

        public async Task<ApiResult<string>> DeleteContact(ContactStatusRequest request, string tokenName)
        {
            var result = await DeleteAsync<string>($"/api/contacts/delete/{request.Id}", tokenName);
            return result;
        }

        public async Task<ApiResult<List<ContactViewModel>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PagedResult<ContactViewModel>>> GetAllPaging(GetContactPagingRequest request, string tokenName)
        {
            var result = await GetAsync<PagedResult<ContactViewModel>>(
                $"/api/contacts/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}&status={request.Status}", tokenName);
            return result;
        }

        public async Task<ApiResult<ContactViewModel>> GetById(int contactId, string tokenName)
        {
            var result = await GetAsync<ContactViewModel>($"/api/contacts/{contactId}", tokenName);
            return result;
        }

        public async Task<ApiResult<string>> HideContact(ContactStatusRequest request, string tokenName)
        {
            var result = await PutAsync<string>($"/api/contacts/hide", request, tokenName);
            return result;
        }

        public async Task<ApiResult<string>> ShowContact(ContactStatusRequest request, string tokenName)
        {
            var result = await PutAsync<string>($"/api/contacts/show", request, tokenName);
            return result;
        }

        public async Task<ApiResult<string>> UpdateContact(ContactUpdateRequest request, string tokenName)
        {
            var result = await PutAsync<string>($"/api/contacts/update", request, tokenName);
            return result;
        }
    }
}
