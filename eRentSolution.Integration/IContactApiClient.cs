using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.Utilities.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public interface IContactApiClient
    {
        Task<ApiResult<List<ContactViewModel>>> GetAll();
        Task<ApiResult<ContactViewModel>> GetById(int contactId, string tokenName);
        Task<ApiResult<string>> AddContact(ContactCreateRequest request);
        Task<ApiResult<string>> DeleteContact(ContactStatusRequest request, string tokenName);
        Task<ApiResult<string>> HideContact(ContactStatusRequest request, string tokenName);
        Task<ApiResult<string>> ShowContact(ContactStatusRequest request, string tokenName);
        Task<ApiResult<string>> UpdateContact(ContactUpdateRequest request, string tokenName);
        Task<ApiResult<PagedResult<ContactViewModel>>> GetAllPaging(GetContactPagingRequest request, string tokenName);
    }
}
