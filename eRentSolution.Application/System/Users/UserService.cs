using eRentSolution.Data.EF;
using eRentSolution.Data.Entities;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly eRentDbContext _context;

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration configuration,
            eRentDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<ApiResult<string>> Authenticate(UserLoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return new ApiErrorResult<string>("Username or password incorrect");
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if(!result.Succeeded)
                return new ApiErrorResult<string>("Username or password incorrect");
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
              _configuration["Tokens:Audience"],
              claims,
              expires: DateTime.Now.AddDays(30),
              signingCredentials: credentials);
            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));

        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new ApiErrorResult<bool>("User is not exist");
            if(user.Id.ToString().Equals(SystemConstant.AppSettings.CurrentUserId))
                return new ApiErrorResult<bool>("Cannot delete current login account");
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Delete user unsuccessful");

        }

        public async Task<ApiResult<UserViewModel>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new ApiErrorResult<UserViewModel>($"Cannot find user with id: {id}");
            var roles = await _userManager.GetRolesAsync(user);
            var person =  await _context.Persons.FirstOrDefaultAsync(x => x.UserId == id);
            var userViewModel = new UserViewModel()
            {
                PhoneNumber = user.PhoneNumber,
                FirstName = person.FirstName,
                Dob = person.Dob,
                Email = user.Email,
                LastName = person.LastName,
                UserName = user.UserName,
                Roles = roles,
                Id = id
            };
            return new ApiSuccessResult<UserViewModel>(userViewModel);
        }

        //public async Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request)
        //{
        //    var query = _userManager.Users;
        //    if (!string.IsNullOrEmpty(request.Keyword))
        //    {
        //        query = query.Where(x => x.UserName.Contains(request.Keyword)
        //                    || x.PhoneNumber.Contains(request.Keyword) 
        //                    || x.UserName.Contains(request.Keyword)
        //                    || x.FirstName.Contains(request.Keyword)
        //                    || x.LastName.Contains(request.Keyword));
        //    }
        //    /* PAGING*/
        //    int totalRow = await query.CountAsync();
        //    var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
        //        .Take(request.PageSize)
        //        .Select(x => new UserViewModel()
        //        {
        //            Id = x.Id,
        //            PhoneNumber = x.PhoneNumber,
        //            FirstName = x.FirstName,
        //            Dob = x.Dob,
        //            Email = x.Email,
        //            LastName = x.LastName,
        //            UserName = x.UserName
        //        }).ToListAsync();

        //    //4.select and projection
        //    var pageResult = new PagedResult<UserViewModel>()
        //    {
        //        TotalRecords = totalRow,
        //        Items = data,
        //        PageIndex = request.PageIndex,
        //        PageSize = request.PageSize
        //    };
        //    return new ApiSuccessResult<PagedResult<UserViewModel>>(pageResult);
        //}
        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request)
        {
            var query = from u in _userManager.Users
                        join p in _context.Persons on u.Id equals p.UserId
                        select new { u, p };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.u.UserName.Contains(request.Keyword)
                            || x.u.PhoneNumber.Contains(request.Keyword)
                            || x.p.LastName.Contains(request.Keyword)
                            || x.p.FirstName.Contains(request.Keyword));
            }
            
            /* PAGING*/
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.u.Id,
                    PhoneNumber = x.u.PhoneNumber,
                    FirstName = x.p.FirstName,
                    Dob = x.p.Dob,
                    Email = x.u.Email,
                    LastName = x.p.LastName,
                    UserName = x.u.UserName
                }).ToListAsync();

            foreach (var item in data)
            {
                
            }
            //4.select and projection
            var pageResult = new PagedResult<UserViewModel>()
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return new ApiSuccessResult<PagedResult<UserViewModel>>(pageResult);
        }
        public async Task<ApiResult<bool>> Register(UserRegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return new ApiErrorResult<bool>("Username already exists");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ApiErrorResult<bool>("Email already exists");
            }
            user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };
            var person = new Person()
            {
                Dob = request.Dob,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (result.Succeeded)
            {
                await _context.Persons.AddAsync(person);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Fail to create account");
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Account does not exists");
            }

            var removedRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            foreach (var roleName in removedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRolesAsync(user, removedRoles);
                }
            }

            var addedRoles = request.Roles.Where(x => x.Selected == true).Select(x => x.Name).ToList();
            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
            {
                return new ApiErrorResult<bool>("Email already exists");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.UserId == id);
            person.Dob = request.Dob;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            person.FirstName = request.FirstName;
            person.LastName = request.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
                
            return new ApiErrorResult<bool>("Update unsuccessful");
        }
    }
}
