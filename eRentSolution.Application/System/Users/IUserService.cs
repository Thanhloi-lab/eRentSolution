﻿using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(UserLoginRequest request);
        Task<ApiResult<bool>> Register(UserRegisterRequest request);
        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);
        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
        Task<ApiResult<UserViewModel>> GetById(Guid id);
        Task<ApiResult<bool>> Delete(Guid id);
    }
}
