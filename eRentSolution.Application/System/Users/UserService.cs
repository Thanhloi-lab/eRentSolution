using eRentSolution.Application.Common;
using eRentSolution.Application.MailServices;
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
using System.Web;

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
        private readonly IMailService _mailService;

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration configuration,
            eRentDbContext context,
            IStorageService storageService,
            IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _storageService = storageService;
            _mailService = mailService;
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

            var result = await _signInManager.PasswordSignInAsync(user, request.Password/* + user.DateChangePassword*/, request.RememberMe, true);
            
            if (!result.Succeeded)
                return new ApiErrorResult<string>("Tài khoản hoặc mật khẩu không đúng");
            
            var userInfo = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == user.Id);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Actor, userInfo.Id.ToString()),
                new Claim(ClaimTypes.Name, userInfo.FirstName),
                new Claim(ClaimTypes.IsPersistent, request.RememberMe.ToString()),
                new Claim(ClaimTypes.GivenName, request.UserName),
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

            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return new ApiSuccessResult<string>("Khóa tài khoản thành công");
                return new ApiErrorResult<string>("Khóa tài khoản thất bại, vui lòng thử lại sau");
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
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
                Dob = request.Dob,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            
            
            try
            {
                var result = await _userManager.CreateAsync(user, request.Password);
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
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            } 
            
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
            try
            {
                var result = await _context.SaveChangesAsync();
                return new ApiSuccessResult<string>("Gán quyền thành công");
            }
            catch(Exception e)
            {
                return new ApiErrorResult<string>("Lỗi");
            }
        }
        public async Task<ApiResult<string>> Update(Guid id, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
            {
                return new ApiErrorResult<string>("Email đã tồn tại");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            //var person = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserId == id);

            user.Dob = request.Dob;
            user.PhoneNumber = request.PhoneNumber;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            if(!user.Email.Equals(request.Email))
            {
                user.EmailConfirmed = false;
            }
            user.Email = request.Email;
            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<string>("Cập nhật thành công");
                }
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
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
            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<string>("Cập nhật ảnh thành công");
                }
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
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
            var date = DateTime.UtcNow;
            try
            {
                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword /*+ user.DateChangePassword*/, request.NewPassword /*+ date)*/);
                if (result.Succeeded)
                {
                    user = await _userManager.FindByIdAsync(request.Id.ToString());
                    await _userManager.UpdateAsync(user);
                    return new ApiSuccessResult<string>("Đổi mật khẩu thành công");
                }
                {
                    return new ApiErrorResult<string>("Mật khẩu phải từ 8 kí tự trở lên và bao gồm kí tự đặt biệt, chữ , số, chữ in hoa.");
                }    
                
                
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }

        }
        public async Task<ApiResult<string>> ResetPassword(UserResetPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<string>("Tài khoản không tồn tại");
            }
            try
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
                if (result.Succeeded)
                    return new ApiSuccessResult<string>("Đặt lại mật khẩu thành công");
                return new ApiErrorResult<string>("Đặt lại mật khẩu thất bại");
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
        }
        public async Task<ApiResult<string>> ResetPasswordByEmail(UserResetPasswordByEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            
            if (user == null)
            {
                return  new ApiErrorResult<string>("Tài khoản không tồn tại");
            }
            //if (!user.Id.ToString().Equals(HttpUtility.UrlDecode(request.Token)))
            //{
            //    return new ApiErrorResult<string>("Tài khoản không tồn tại");
            //}
            if (!user.EmailConfirmed)
            {
                return new ApiErrorResult<string>("Email chưa được xác thực.");
            }
            var date = DateTime.UtcNow;
            if (request.Date.CompareTo(date) > 0)
            {
                request.Token = request.Token.Replace(" ", "+");
                var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
                //var removeResult = await _userManager.RemovePasswordAsync(user);
                ////user.DateChangePassword = date;
                if (result.Succeeded)
                {
                    //var result = await _userManager.AddPasswordAsync(user, request.Password /*+ date*/);
                    //if (result.Succeeded)
                    //{
                    //    //user.DateChangePassword = date;
                    //    await _userManager.UpdateAsync(user);
                        return new ApiSuccessResult<string>("Đặt lại mật khẩu thành công");
                    //}
                    //else
                    //    return new ApiErrorResult<string>("Đặt lại mật khẩu thất bại");
                }
                return new ApiErrorResult<string>("Mã xác nhận đã mất hiệu lực hoặc mật khẩu phải từ 8 kí tự trở lên và bao gồm kí tự đặt biệt, chữ, số, chữ in hoa.");
            }
            return new ApiErrorResult<string>("Đặt lại mật khẩu thất bại");
        }
        public async Task<ApiResult<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = _context.Users.Where(x => x.Email == request.Email).FirstOrDefault();
            if (user != null) 
            {
                if(!user.EmailConfirmed)
                {
                    return new ApiErrorResult<string>("Email không khả dụng");
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var date = HttpUtility.UrlEncode(DateTime.UtcNow.AddMinutes(10).ToString());
                //var token = HttpUtility.UrlEncode(user.Id.ToString());
                var schema = request.CurrentDomain;
                var url = $"{schema}/user/ResetPasswordByEmail?expires={date}&Email={request.Email}&Token={token}";
                string message = string.Format("<p>Nhấn vào đây để khôi phục mật khẩu</p><a href = \"{0}\" >Link </a>", url);
                await _mailService.SendEmailAsync(request.Email, "Khôi phục mật khẩu", message);
                return new ApiSuccessResult<string>("Đã gửi mail thành công.");
            }
            else
                return new ApiErrorResult<string>("Email không tồn tại");

        }
        public async Task<ApiResult<string>> SendConfirmEmail(SendConfirmEmailRequest request)
        {
            var user = _context.Users.Where(x => x.Email == request.Email).FirstOrDefault();
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    return new ApiErrorResult<string>("Email đã xác thực");
                }
                var date = HttpUtility.UrlEncode(DateTime.UtcNow.AddMinutes(10).ToString());
                //var token = HttpUtility.UrlEncode(user.Id.ToString());
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var schema = request.CurrentDomain;
                var url = $"{schema}/user/confirmEmail?expires={date}&Email={request.Email}&Token={token}";
                string message = string.Format("<p>Nhấn vào đây để xác nhận email</p><a href = \"{0}\" >Link </a>", url);
                await _mailService.SendEmailAsync(request.Email, "Xác nhận email", message);
                return new ApiSuccessResult<string>("Đã gửi mail thành công.");

            }
            else
                return new ApiErrorResult<string>("Email không tồn tại");

        }
        public async Task<ApiResult<string>> ConfirmEmail(ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ApiErrorResult<string>("Tài khoản không tồn tại");
            }
            //if (!user.Id.ToString().Equals(HttpUtility.UrlDecode(request.Token)))
            //{
            //    return new ApiErrorResult<string>("Tài khoản không tồn tại");
            //}
            if (user.EmailConfirmed)
            {
                return new ApiErrorResult<string>("Email đã được xác thực.");
            }
            var date = DateTime.UtcNow;
            if (request.Date.CompareTo(date) < 0)
            {
                return new ApiErrorResult<string>("Mã xác thực đã hết hạn.");
            }
            try
            {
                request.Token = request.Token.Replace(" ", "+");
                var result = await _userManager.ConfirmEmailAsync(user, request.Token);
                if (result.Succeeded)
                {
                    return new ApiSuccessResult<string>("Xác thực thành công");
                }
                else
                    return new ApiErrorResult<string>("Xác thực thất bại.");
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Có lỗi trong quá trình thực hiện");
            }
        }
        public async Task<ApiResult<UserViewModel>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new ApiErrorResult<UserViewModel>($"Tài khoản không tồn tại");
            var roles = await _userManager.GetRolesAsync(user);
            //var person = await _context.UserInfos.FirstOrDefaultAsync(x => x.UserId == id);
            var userViewModel = new UserViewModel()
            {
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                Dob = user.Dob,
                Email = user.Email,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = roles,
                Id = id,
                AvatarFilePath = user.AvatarFilePath,
                Status = user.Status,
                EmailConfirmed = user.EmailConfirmed
            };
            return new ApiSuccessResult<UserViewModel>(userViewModel);
        }
        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request)
        {
            var query = from u in _userManager.Users
                        where u.Status == Status.Active
                        select new { u};
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.u.UserName.Contains(request.Keyword)
                            || x.u.PhoneNumber.Contains(request.Keyword)
                            || x.u.LastName.Contains(request.Keyword)
                            || x.u.FirstName.Contains(request.Keyword));
            }

            /* PAGING*/
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .OrderBy(x => x.u.LastName)
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.u.Id,
                    PhoneNumber = x.u.PhoneNumber,
                    FirstName = x.u.FirstName,
                    Dob = x.u.Dob,
                    Email = x.u.Email,
                    LastName = x.u.LastName,
                    UserName = x.u.UserName,
                    AvatarFilePath = x.u.AvatarFilePath,
                    Status = x.u.Status,
                    EmailConfirmed =x.u.EmailConfirmed
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
                        //join ui in _context.UserInfos on u.Id equals ui.UserId
                        join ur in _context.UserRoles on u.Id equals ur.UserId
                        join r in _roleManager.Roles on ur.RoleId equals r.Id
                        where u.Status == Status.Active
                        select new { u, r };
            
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.u.UserName.Contains(request.Keyword)
                            || x.u.PhoneNumber.Contains(request.Keyword)
                            || x.u.LastName.Contains(request.Keyword)
                            || x.u.FirstName.Contains(request.Keyword));
            }

            /* PAGING*/
            int totalRow = await query.CountAsync();
            var data = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .OrderBy(x => x.u.LastName)
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.u.Id,
                    PhoneNumber = x.u.PhoneNumber,
                    FirstName = x.u.FirstName,
                    Dob = x.u.Dob,
                    Email = x.u.Email,
                    LastName = x.u.LastName,
                    UserName = x.u.UserName,
                    AvatarFilePath = x.u.AvatarFilePath,
                    Status = x.u.Status,
                    EmailConfirmed = x.u.EmailConfirmed
                }).Distinct().ToListAsync();

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
            var query = from ui in _context.AppUsers
                        join c in _context.Censors on ui.Id equals c.UserId
                        join a in _context.UserActions on c.ActionId equals a.Id
                        join p in _context.News on c.NewsId equals p.Id
                        where ui.Id == request.Id
                        select new { ui, a, p, c };

            var totalRow = await query.CountAsync();
            var data = await query.OrderBy(x => x.c.Date)
                .Skip((request.PageIndex - 1) * request.PageSize)
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
        public async Task<ApiResult<PagedResult<ActivityLogViewModel>>> GetPageUserActivities(UserActivityLogRequest request)
        {
            var query = from ui in _context.AppUsers
                        join c in _context.Censors on ui.Id equals c.UserId
                        join a in _context.UserActions on c.ActionId equals a.Id
                        join p in _context.News on c.NewsId equals p.Id
                        select new { ui, a, p, c };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.ui.LastName.Contains(request.Keyword)
                            || x.p.Name.Contains(request.Keyword)
                            || x.a.ActionName.Contains(request.Keyword));
            }

            var totalRow = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.c.Date)
                .Skip((request.PageIndex - 1) * request.PageSize)
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
            var query = (from u in _context.AppUsers
                         join c in _context.Censors on u.Id equals c.UserId
                         join a in _context.UserActions on c.ActionId equals a.Id
                         join p in _context.News on c.NewsId equals p.Id
                         where p.Id == productId && a.ActionName == action.ActionName
                         select new { u }).FirstOrDefault();
            var user = new UserViewModel()
            {
                AvatarFilePath = query.u.AvatarFilePath,
                Dob = query.u.Dob,
                Email = query.u.Email,
                FirstName = query.u.FirstName,
                Id = query.u.Id,
                LastName = query.u.LastName,
                PhoneNumber = query.u.PhoneNumber,
                UserName = query.u.UserName
            };
            return new ApiSuccessResult<UserViewModel>(user);
        }
        public async Task<ApiResult<string>> RefreshToken(UserLoginRequest request, bool isAdminPage)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return new ApiErrorResult<string>("Đã có lỗi trong quá trình. Vui lòng đăng nhập lại");

            if (user.Status == Data.Enums.Status.InActive)
                return new ApiErrorResult<string>("Tài khoản đã bị khóa");

            var roles = await _userManager.GetRolesAsync(user);
            if (isAdminPage)
            {
                if (!roles.Contains(SystemConstant.AppSettings.UserAdminRole) && !roles.Contains(SystemConstant.AppSettings.AdminRole))
                    return new ApiErrorResult<string>("Đã có lỗi trong quá trình. Vui lòng đăng nhập lại");
            }

            //var userInfo = await _context.UserInfos.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Actor, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.IsPersistent, request.RememberMe.ToString()),
                new Claim(ClaimTypes.GivenName, request.UserName),
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
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
