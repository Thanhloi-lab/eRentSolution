using eRentSolution.ViewModels.Common;
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

        public async Task<ApiResult<List<RoleViewModel>>> GetAll(string tokenName)
        {
            var result = await GetListAsync<RoleViewModel>($"api/roles", tokenName);
            return result;
        }
    }
}
