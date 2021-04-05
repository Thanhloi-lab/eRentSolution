using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public interface IUserApiClient
    {
        public Task<ApiResult<string>> Authenticate(UserLoginRequest login, bool isAdminPage);
        public Task<PagedResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request);
        public Task<ApiResult<bool>> RegisterUser(UserRegisterRequest registerRequest);
        Task<UserViewModel> GetById(Guid id);
        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);
        Task<ApiResult<bool>> UpdatePassword(UserUpdatePasswordRequest request);
        Task<ApiResult<bool>> ResetPassword(UserResetPasswordRequest request);
        Task<bool> Delete(Guid id);
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}
