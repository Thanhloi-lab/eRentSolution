using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.Utilities.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.Utilities.Contacts
{
    public interface IContactService
    {
        Task<ApiResult<List<ContactViewModel>>> GetAll();
        Task<ApiResult<ContactViewModel>> GetById(int contactId);
        Task<ApiResult<string>> AddContact(ContactCreateRequest request);
        Task<ApiResult<string>> DeleteContact(ContactStatusRequest request);
        Task<ApiResult<string>> HideContact(ContactStatusRequest request);
        Task<ApiResult<string>> ShowContact(ContactStatusRequest request);
        Task<ApiResult<string>> UpdateContact(ContactUpdateRequest request);
        Task<ApiResult<PagedResult<ContactViewModel>>> GetAllPaging(GetContactPagingRequest request);
    }
}
