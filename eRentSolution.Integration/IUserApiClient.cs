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
        Task<ApiResult<string>> Authenticate(UserLoginRequest login, bool isAdminPage);
        Task<ApiResult<string>> Update(UserUpdateRequest request, string tokenName);
        Task<ApiResult<string>> UpdateAvatar(UserAvatarUpdateRequest request, string tokenName);
        Task<ApiResult<string>> UpdatePassword(UserUpdatePasswordRequest request, string tokenName);
        Task<ApiResult<string>> ForgotPassword(ForgotPasswordRequest request);
        Task<ApiResult<string>> ResetPassword(UserResetPasswordRequest request, string tokenName);
        Task<ApiResult<string>> ResetPasswordByEmail(UserResetPasswordByEmailRequest request, string tokenName);
        Task<ApiResult<string>> SendConfirmEmail(SendConfirmEmailRequest request, string tokenName);
        Task<ApiResult<string>> ConfirmEmail(ConfirmEmailRequest request, string tokenName);
        Task<ApiResult<string>> Delete(Guid id, string tokenName);
        Task<ApiResult<string>> BanUser(Guid id, string tokenName);
        Task<ApiResult<string>> RoleAssign(RoleAssignRequest request, string tokenName);
        Task<ApiResult<string>> RegisterUser(UserRegisterRequest registerRequest);
        Task<ApiResult<string>> RefreshToken(UserLoginRequest login, bool isAdminPage);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPaging(GetUserPagingRequest request, string tokenName);
        Task<ApiResult<PagedResult<UserViewModel>>> GetStaffsPaging(GetUserPagingRequest request, string tokenName);
        Task<ApiResult<UserViewModel>> GetById(Guid id, string tokenName);
        Task<ApiResult<UserViewModel>> GetUserByProductId(int productId, string tokenName);
        Task<ApiResult<PagedResult<ActivityLogViewModel>>> GetUserActivities(UserActivityLogRequest request, string tokenName);
        Task<ApiResult<PagedResult<ActivityLogViewModel>>> GetPageActivities(UserActivityLogRequest request, string tokenName);
    }
}
