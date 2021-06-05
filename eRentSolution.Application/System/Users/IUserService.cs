using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(UserLoginRequest request, bool isAdminPage);
        Task<ApiResult<string>> RefreshToken(UserLoginRequest request, bool isAdminPage);
        Task<ApiResult<string>> Register(UserRegisterRequest request);
        Task<ApiResult<string>> Update(Guid id, UserUpdateRequest request);
        Task<ApiResult<string>> UpdatePassword(UserUpdatePasswordRequest request);
        Task<ApiResult<string>> ResetPassword(UserResetPasswordRequest request);
        Task<ApiResult<string>> ResetPasswordByEmail(UserResetPasswordByEmailRequest request);
        Task<ApiResult<string>> RoleAssign(Guid id, RoleAssignRequest request);
        Task<ApiResult<string>> Delete(Guid id);
        Task<ApiResult<string>> UpdateAvatar(UserAvatarUpdateRequest request);
        Task<ApiResult<string>> BanUser(Guid id);
        Task<ApiResult<string>> ForgotPassword(ForgotPasswordRequest request);
        Task<ApiResult<string>> SendConfirmEmail(SendConfirmEmailRequest request);
        Task<ApiResult<string>> ConfirmEmail(ConfirmEmailRequest request);
        Task<ApiResult<PagedResult<UserViewModel>>> GetStaffPaging(GetUserPagingRequest request);
        Task<ApiResult<UserViewModel>> GetById(Guid id);
        Task<ApiResult<UserViewModel>> GetUserByProductId(int productId);
        Task<ApiResult<PagedResult<ActivityLogViewModel>>> GetUserActivities(UserActivityLogRequest request);
        Task<ApiResult<PagedResult<ActivityLogViewModel>>> GetPageUserActivities(UserActivityLogRequest request);
        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);

    }
}
