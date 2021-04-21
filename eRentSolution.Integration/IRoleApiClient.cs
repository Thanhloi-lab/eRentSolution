using eRentSolution.ViewModels.System.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public interface IRoleApiClient
    {
        Task<List<RoleViewModel>> GetAll(string tokenName);
    }
}
