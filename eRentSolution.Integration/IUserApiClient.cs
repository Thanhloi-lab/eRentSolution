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
        public Task<PagedResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request, string tokenName);
        public Task<ApiResult<bool>> RegisterUser(UserRegisterRequest registerRequest);
        Task<UserViewModel> GetById(Guid id, string tokenName);
        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request, string tokenName);
        Task<ApiResult<bool>> UpdatePassword(UserUpdatePasswordRequest request, string tokenName);
        Task<ApiResult<bool>> ResetPassword(UserResetPasswordRequest request, string tokenName);
        Task<bool> Delete(Guid id, string tokenName);
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request, string tokenName);
        Task<PagedResult<ActivityLogViewModel>> GetUserActivities(UserActivityLogRequest request, string tokenName);
        Task<PagedResult<ActivityLogViewModel>> GetPageActivities(UserActivityLogRequest request, string tokenName);
    }
}
