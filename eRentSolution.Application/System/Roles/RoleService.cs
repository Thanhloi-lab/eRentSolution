using eRentSolution.Data.Entities;
using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {
            var roles = await _roleManager.Roles
                .Select(x=>new RoleViewModel() 
                {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            if (roles == null)
                return new ApiErrorResult<List<RoleViewModel>>("Không tồn tại vai trò nào");
            return new ApiSuccessResult<List<RoleViewModel>>(roles);
        }
    }
}
