using eRentSolution.Application.Common;
using eRentSolution.Data.EF;
using eRentSolution.Data.Entities;
using eRentSolution.Data.Enums;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
        private readonly IStorageService _storageService;

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration configuration,
            eRentDbContext context,
            IStorageService storageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<string>> Authenticate(UserLoginRequest request, bool isAdminPage)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return new ApiErrorResult<string>("Tài khoản hoặc mật khẩu không đúng");

            if (user.Status == Data.Enums.Status.InActive)
                return new ApiErrorResult<string>("Tài khoản đã bị khóa");

            var roles = await _userManager.GetRolesAsync(user);
            if(isAdminPage)
            {
                if(!roles.Contains(SystemConstant.AppSettings.UserAdminRole) && !roles.Contains(SystemConstant.AppSettings.AdminRole))
                    return new ApiErrorResult<string>("Tài khoản hoặc mật khẩu không đúng");
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password + user.DateChangePassword, request.RememberMe, true);
            if(!result.Succeeded)
                return new ApiErrorResult<string>("Tài khoản hoặc mật khẩu không đúng");
            
            var userInfo = await _context.UserInfos.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Actor, userInfo.UserId.ToString()),
                new Claim(ClaimTypes.Name, userInfo.FirstName),
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
              expires: DateTime.UtcNow.AddDays(30),
              signingCredentials: credentials);
            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));

        }
        public async Task<ApiResult<string>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new ApiErrorResult<string>("Tài khoản không tồn tại");
            if(user.Id.ToString().Equals(SystemConstant.AppSettings.CurrentUserId))
                return new ApiErrorResult<string>("Không thể xóa tài khoản hiện tại");

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return new ApiSuccessResult<string>("Xóa thành công");
            return new ApiErrorResult<string>("Xóa thất bại, vui lòng thử lại sau");

        }
        public async Task<ApiResult<string>> BanUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new ApiErrorResult<string>("Tài khoản không tồn tại");
            if (user.Id.ToString().Equals(SystemConstant.AppSettings.CurrentUserId))
                return new ApiErrorResult<string>("Không thể khóa tài khoản hiện tại");

            user.Status = Data.Enums.Status.InActive;
            
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return new ApiSuccessResult<string>("Khóa tài khoản thành công");
            return new ApiErrorResult<string>("Khóa tài khoản thất bại, vui lòng thử lại sau");

        }
        public async Task<ApiResult<string>> Register(UserRegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return new ApiErrorResult<string>("Tài khoản đã tồn tại");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ApiErrorResult<string>("Email đã tồn tại");
            }
            
            user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DateChangePassword = DateTime.UtcNow,
                Person = new UserInfo()
                {
                    Dob = request.Dob,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                }
            };
            
            var result = await _userManager.CreateAsync(user, request.Password + user.DateChangePassword);
            if (result.Succeeded)
            {
                var addedRoles = request.Roles.Where(x => x.Selected == true).Select(x => x.Name).ToList();
                foreach (var roleName in addedRoles)
                {
                    if (await _userManager.IsInRoleAsync(user, roleName) == false)
                    {
                        await _userManager.AddToRoleAsync(user, roleName);
                    }
                }
                user.AvatarFilePath = SystemConstant.DefaultAvatar;
                user.AvatarFileSize = SystemConstant.DefaultAvatarSize;
                await _userManager.UpdateAsync(user);
                return new ApiSuccessResult<string>("Tạo thành công");
            }    
            return new ApiErrorResult<string>("Tại tài khoản thất bại");
        }
        public async Task<ApiResult<string>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<string>("Tài khoản không tồn tại");
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

            return new ApiSuccessResult<string>("Gán quyền thành công");
        }
        public async Task<ApiResult<string>> Update(Guid id, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
            {
                return new ApiErrorResult<string>("Email không tồn tại");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            var person = await _context.UserInfos.FirstOrDefaultAsync(x => x.UserId == id);
            person.Dob = request.Dob;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            person.FirstName = request.FirstName;
            person.LastName = request.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<string>("Cập nhật thành công");
            }
                
            return new ApiErrorResult<string>("Cập nhật thất bại");
        }
        public async Task<ApiResult<string>> UpdateAvatar(UserAvatarUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            int isDeleteSuccess = 0;
            if(request.AvatarFile!=null)
            {
                if(user.AvatarFilePath != SystemConstant.DefaultAvatar && user.AvatarFilePath!=null)
                {
                    isDeleteSuccess =  _storageService.DeleteFile(user.AvatarFilePath);
                }
                if (isDeleteSuccess == -1)
                    new ApiErrorResult<string>("Cập nhật ảnh thất bại, vui lòng thử lại sau");

                user.AvatarFilePath = await this.SaveFile(request.AvatarFile);
            }
            
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<string>("Cập nhật ảnh thành công");
            }

            return new ApiErrorResult<string>("Cập nhật ảnh thất bại");
        }
        public async Task<ApiResult<string>> UpdatePassword(UserUpdatePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if(user == null)
            {
                new ApiErrorResult<string>("Tài khoản không tồn tại");
            }
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword+user.DateChangePassword, request.NewPassword + DateTime.UtcNow);
            if (result.Succeeded)
            {
               // await _context.SaveChangesAsync();
                return new ApiSuccessResult<string>("Đổi mật khẩu thành công");
            }

            return new ApiErrorResult<string>("Đổi mật khẩu thất bại, vui lòng thử lại sau");
        }
        public async Task<ApiResult<string>> ResetPassword(UserResetPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                new ApiErrorResult<string>("Tài khoản không tồn tại");
            }
            user.DateChangePassword = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (removeResult.Succeeded)
            {
                var result = await _userManager.AddPasswordAsync(user, request.NewPassword + user.DateChangePassword);
                return new ApiSuccessResult<string>("Đặt lại mật khẩu thành công");
            }
            return new ApiErrorResult<string>("Đặt lại mật khẩu thất bại");
        }
        public async Task<ApiResult<UserViewModel>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new ApiErrorResult<UserViewModel>($"Tài khoản không tồn tại");
            var roles = await _userManager.GetRolesAsync(user);
            var person = await _context.UserInfos.FirstOrDefaultAsync(x => x.UserId == id);
            var userViewModel = new UserViewModel()
            {
                PhoneNumber = user.PhoneNumber,
                FirstName = person.FirstName,
                Dob = person.Dob,
                Email = user.Email,
                LastName = person.LastName,
                UserName = user.UserName,
                Roles = roles,
                Id = id,
                AvatarFilePath = user.AvatarFilePath,
                Status = user.Status
            };
            return new ApiSuccessResult<UserViewModel>(userViewModel);
        }
        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request)
        {
            var query = from u in _userManager.Users
                        join ui in _context.UserInfos on u.Id equals ui.UserId
                        select new { u, ui };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.u.UserName.Contains(request.Keyword)
                            || x.u.PhoneNumber.Contains(request.Keyword)
                            || x.ui.LastName.Contains(request.Keyword)
                            || x.ui.FirstName.Contains(request.Keyword));
            }

            /* PAGING*/
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .OrderBy(x => x.ui.LastName)
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.u.Id,
                    PhoneNumber = x.u.PhoneNumber,
                    FirstName = x.ui.FirstName,
                    Dob = x.ui.Dob,
                    Email = x.u.Email,
                    LastName = x.ui.LastName,
                    UserName = x.u.UserName,
                    AvatarFilePath = x.u.AvatarFilePath,
                    Status = x.u.Status
                }).ToListAsync();

            var pageResult = new PagedResult<UserViewModel>()
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return new ApiSuccessResult<PagedResult<UserViewModel>>(pageResult);
        }
        public async Task<ApiResult<PagedResult<UserViewModel>>> GetStaffPaging(GetUserPagingRequest request)
        {
           
            var query = from u in _userManager.Users
                        join ui in _context.UserInfos on u.Id equals ui.UserId
                        join ur in _context.UserRoles on u.Id equals ur.UserId
                        join r in _roleManager.Roles on ur.RoleId equals r.Id
                        select new { u, ui, r };
            
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.u.UserName.Contains(request.Keyword)
                            || x.u.PhoneNumber.Contains(request.Keyword)
                            || x.ui.LastName.Contains(request.Keyword)
                            || x.ui.FirstName.Contains(request.Keyword));
            }

            /* PAGING*/
            int totalRow = await query.CountAsync();
            var data = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .OrderBy(x => x.ui.LastName)
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.u.Id,
                    PhoneNumber = x.u.PhoneNumber,
                    FirstName = x.ui.FirstName,
                    Dob = x.ui.Dob,
                    Email = x.u.Email,
                    LastName = x.ui.LastName,
                    UserName = x.u.UserName,
                    AvatarFilePath = x.u.AvatarFilePath,
                    Status = x.u.Status
                }).ToListAsync();

            var pageResult = new PagedResult<UserViewModel>()
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return new ApiSuccessResult<PagedResult<UserViewModel>>(pageResult);
        }
        public async Task<ApiResult<PagedResult<ActivityLogViewModel>>> GetUserActivities(UserActivityLogRequest request)
        {
            var query = from ui in _context.UserInfos
                        join c in _context.Censors on ui.UserId equals c.UserInfoId
                        join a in _context.UserActions on c.ActionId equals a.Id
                        join p in _context.Products on c.ProductId equals p.Id
                        where ui.UserId == request.Id
                        select new { ui, a, p, c };

            var totalRow = await query.CountAsync();
            var data = await query.Select(x => new ActivityLogViewModel()
            {
                ActionName = x.a.ActionName,
                Date = x.c.Date,
                ProductName = x.p.Name,
                UserLastName = x.ui.LastName
            }).ToListAsync();

            var pageResult = new PagedResult<ActivityLogViewModel>()
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return new ApiSuccessResult<PagedResult<ActivityLogViewModel>>(pageResult);
        }
        public async Task<ApiResult<PagedResult<ActivityLogViewModel>>> GetPageUserActivities(UserActivityLogRequest request)
        {
            var query = from ui in _context.UserInfos
                        join c in _context.Censors on ui.UserId equals c.UserInfoId
                        join a in _context.UserActions on c.ActionId equals a.Id
                        join p in _context.Products on c.ProductId equals p.Id
                        select new { ui, a, p, c };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.ui.LastName.Contains(request.Keyword)
                            || x.p.Name.Contains(request.Keyword)
                            || x.a.ActionName.Contains(request.Keyword));
            }

            var totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .OrderBy(x => x.c.Date)
                .Take(request.PageSize)
                .Select(x => new ActivityLogViewModel()
            {
                ActionName = x.a.ActionName,
                Date = x.c.Date,
                ProductName = x.p.Name,
                UserLastName = x.ui.LastName
            }).ToListAsync();

            var pageResult = new PagedResult<ActivityLogViewModel>()
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return new ApiSuccessResult<PagedResult<ActivityLogViewModel>>(pageResult);
        }
        public async Task<ApiResult<UserViewModel>> GetUserByProductId(int productId)
        {
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProduct);
            var query = (from ui in _context.UserInfos
                         join u in _context.AppUsers on ui.UserId equals u.Id
                         join c in _context.Censors on ui.UserId equals c.UserInfoId
                         join a in _context.UserActions on c.ActionId equals a.Id
                         join p in _context.Products on c.ProductId equals p.Id
                         where p.Id == productId && a.ActionName == action.ActionName
                         select new { u, ui }).FirstOrDefault();
            var user = new UserViewModel()
            {
                AvatarFilePath = query.u.AvatarFilePath,
                Dob = query.ui.Dob,
                Email = query.u.Email,
                FirstName = query.ui.FirstName,
                Id = query.u.Id,
                LastName = query.ui.LastName,
                PhoneNumber = query.u.PhoneNumber,
                UserName = query.u.UserName
            };
            return new ApiSuccessResult<UserViewModel>(user);
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public Task<ApiResult<string>> RefreshToken(UserViewModel userViewModel, bool isAdminPage)
        {
            throw new NotImplementedException();
        }
    }
}
