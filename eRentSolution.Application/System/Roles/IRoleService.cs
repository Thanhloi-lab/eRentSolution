using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.System.Roles
{
    public interface IRoleService
    {
        Task<ApiResult<List<RoleViewModel>>> GetAll();
    }
}
