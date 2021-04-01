using eRentSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public class RoleApiClient : BaseApiClient ,IRoleApiClient
    {
        public RoleApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var result = await GetListAsync<RoleViewModel>($"api/roles");
            return result;
        }
    }
}
