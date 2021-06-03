using eRentSolution.Data.EF;
using eRentSolution.Data.Entities;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.Utilities.Contacts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.Utilities.Contacts
{
    public class ContactService : IContactService
    {
        private readonly eRentDbContext _context;

        public ContactService(eRentDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<string>> AddContact(ContactCreateRequest request)
        {

            var contact = new Contact()
            {
                Name = request.Name,
                Email = request.Email,
                Message = request.Message,
                PhoneNumber = request.PhoneNumber,
                Status = Data.Enums.Status.InActive
            };
            await _context.Contacts.AddAsync(contact);

            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }

            
            if (result > 0)
                return new ApiSuccessResult<string>("Thêm thông tin liên hệ thành công");
            return new ApiErrorResult<string>("Thêm thông tin liên hệ thất bại");
        }
        public async Task<ApiResult<string>> DeleteContact(ContactStatusRequest request)
        {
            var contact = await _context.Slides.FindAsync(request.Id);
            if (contact == null)
                return new ApiErrorResult<string>("Thông tin liên hệ không tồn tại");

            _context.Slides.Remove(contact);

            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
                return new ApiSuccessResult<string>("Xóa thông tin liên hệ thành công");
            return new ApiErrorResult<string>("Xóa thông tin liên hệ thất bại");
        }
        public Task<ApiResult<List<ContactViewModel>>> GetAll()
        {
            throw new NotImplementedException();
        }
        public async Task<ApiResult<PagedResult<ContactViewModel>>> GetAllPaging(GetContactPagingRequest request)
        {
            var query = from ct in _context.Contacts
                        select new { ct };
            if (request.Status != null && request.Status.HasValue)
            {
                if (request.Status == 0)
                    query = query.Where(x => x.ct.Status == Data.Enums.Status.InActive);
                else
                    query = query.Where(x => x.ct.Status == Data.Enums.Status.Active);
            }
            int count = query.Count();
            var contacts = await query.OrderBy(x=>x.ct.Id).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                .Select(x => new ContactViewModel()
                {
                    Id = x.ct.Id,
                    Name = x.ct.Name,
                    Status = x.ct.Status,
                    Message = x.ct.Message,
                    Email =x.ct.Email,
                    PhoneNumber = x.ct.PhoneNumber
                }).ToListAsync();
            
            var page = new PagedResult<ContactViewModel>()
            {
                Items = contacts,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = count
            };

            if (contacts == null)
                return new ApiErrorResult<PagedResult<ContactViewModel>>("Không tồn tại thông tin liên hệ nào");
            return new ApiSuccessResult<PagedResult<ContactViewModel>>(page);
        }
        public async Task<ApiResult<ContactViewModel>> GetById(int contactId)
        {
            var contact = await _context.Contacts.FindAsync(contactId);
            if (contact == null)
                return new ApiErrorResult<ContactViewModel>("Thông tin liên hệ không tồn tại");

            var contactViewModel = new ContactViewModel()
            {
                Id = contact.Id,
                Email = contact.Email,
                Message = contact.Message,
                Name = contact.Name,
                PhoneNumber = contact.PhoneNumber,
                Status = contact.Status
            };

            return new ApiSuccessResult<ContactViewModel>(contactViewModel);
        }
        public async Task<ApiResult<string>> HideContact(ContactStatusRequest request)
        {
            var contact = await _context.Contacts.FindAsync(request.Id);
            if (contact == null)
                return new ApiErrorResult<string>("Thông tin liên hệ không tồn tại");

            contact.Status = Data.Enums.Status.InActive;
            _context.Contacts.Update(contact);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
                return new ApiSuccessResult<string>("Ẩn thông tin liên hệ thành công");
            return new ApiErrorResult<string>("Ẩn thông tin liên hệ thất bại");
            
        }
        public async Task<ApiResult<string>> ShowContact(ContactStatusRequest request)
        {
            var contact = await _context.Contacts.FindAsync(request.Id);
            if (contact == null)
                return new ApiErrorResult<string>("Thông tin liên hệ không tồn tại");

            contact.Status = Data.Enums.Status.Active;
            _context.Contacts.Update(contact);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
                return new ApiSuccessResult<string>("Hiện thông tin liên hệ thành công");
            return new ApiErrorResult<string>("Hiện thông tin liên hệ thất bại");
            
        }
        public async Task<ApiResult<string>> UpdateContact(ContactUpdateRequest request)
        {
            var contact = await _context.Contacts.FindAsync(request.Id);
            if (contact == null)
                return new ApiErrorResult<string>("Thông tin liên hệ không tồn tại");

            contact.Name = request.Name;
            contact.Email = request.Email;
            contact.Message = request.Message;
            contact.PhoneNumber = request.PhoneNumber;

            _context.Contacts.Update(contact);
           
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if(result >0)
                return new ApiSuccessResult<string>("Cập nhật thông tin liên hệ thành công");
            return new ApiErrorResult<string>("Cập nhật thông tin liên hệ thất bại");
        }
    }
}
