using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleViewModel>>> GetAll(string tokenName);
    }
}
