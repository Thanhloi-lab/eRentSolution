using eRentSolution.Data.EF;
using eRentSolution.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eRentSolution.Data.Entities;
using eRentSolution.Application.Common;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eRentSolution.ViewModels.Common;
using eRentSolution.Utilities.Constants;

namespace eRentSolution.Application.Utilities.Slides
{
    public class SlideService : ISlideService
    {
        private readonly eRentDbContext _context;
        private readonly IStorageService _storageService;
        private string productUrlPattern = SystemConstant.BackendApiProductUrl;

        public SlideService(eRentDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<string>> AddSlide(SlideCreateRequest request, Guid userInfoId)
        {
            var action = await _context.UserActions
                .FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateSlide);
            
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
                return new ApiErrorResult<string>("Sản phẩm không tồn tại");

            var slide = new Slide()
            {
                Name = request.Name,
                Description = request.Description,
                ImagePath = await this.SaveFile(request.ImageFile),
                ProductId = request.ProductId,
                Url = productUrlPattern + request.ProductId,
                Status = Data.Enums.Status.Active
            };
            await _context.Slides.AddAsync(slide);
            var result = await _context.SaveChangesAsync();
            if (result >0)
            {
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    UserInfoId = userInfoId,
                    Date = DateTime.UtcNow,
                    ProductId = product.Id
                };
                 await _context.Censors.AddAsync(censor);
            }
            else
            {
                return new ApiErrorResult<string>("Thêm sản phẩm trình chiếu thất bại");
            }    
            result = await _context.SaveChangesAsync();
            if (result > 0)
                return new ApiSuccessResult<string>("Thêm sản phẩm trình chiếu thành công");
            return new ApiErrorResult<string>("Thêm sản phẩm trình chiếu thất bại");
        }
        public async Task<ApiResult<string>> HideSlide(SlideStatusRequest request, Guid userInfoId)
        {
            var action = await _context.UserActions
                .FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.HideSlide);
            
            var slide = await _context.Slides.FindAsync(request.Id);
            if (slide == null)
                return new ApiErrorResult<string>("Sản phẩm trình chiếu không tồn tại");

            slide.Status = Data.Enums.Status.InActive;
            var result = await _context.SaveChangesAsync();
            
            if (result < 0)
                return new ApiErrorResult<string>("Ẩn sản phâm trình chiếu thất bại");
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserInfoId = userInfoId,
                Date = DateTime.UtcNow,
                ProductId = slide.ProductId
            };
            await _context.Censors.AddAsync(censor);
            result = await _context.SaveChangesAsync();
            if (result < 0)
                return new ApiErrorResult<string>("Ẩn sản phâm trình chiếu thất bại");
            return new ApiSuccessResult<string>("Ẩn sản phẩm trình chiếu thành công");
        }
        public async Task<ApiResult<string>> DeleteSlide(SlideStatusRequest request, Guid userInfoId)
        {
            var slide = await _context.Slides.FindAsync(request.Id);
            if (slide == null)
                return new ApiErrorResult<string>("Sản phẩm trình chiếu không tồn tại");

            _storageService.DeleteFile(slide.ImagePath);
            _context.Slides.Remove(slide);
            var result = await _context.SaveChangesAsync();
            if(result >0)
            {
                var action =await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.DeleteSlide);
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    UserInfoId = userInfoId,
                    Date = DateTime.UtcNow,
                    ProductId = slide.ProductId
                };
                await _context.Censors.AddAsync(censor);
                result = await _context.SaveChangesAsync();
                if(result > 0)
                    return new ApiSuccessResult<string>("Xóa sản phẩm trình chiếu thành công");
                return new ApiErrorResult<string>("Xóa sản phâm trình chiếu thất bại");
            }
            return new ApiErrorResult<string>("Xóa sản phâm trình chiếu thất bại");
        }
        public async Task<ApiResult<string>> UpdateSlide(SlideUpdateRequest request, Guid userInfoId)
        {
            var action = await _context.UserActions
                .FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateSlide);

            var slide = await _context.Slides.FindAsync(request.Id);
            if (slide == null)
                return new ApiErrorResult<string>("Sản phẩm trình chiếu không tồn tại");
            slide.Description = request.Description;
            slide.Name = request.Name;

            var result = await _context.SaveChangesAsync();
            if (result < 0)
                return new ApiErrorResult<string>("Cập nhật sản phâm trình chiếu thất bại");

            var censor = new Censor()
            {
                ActionId = action.Id,
                UserInfoId = userInfoId,
                Date = DateTime.UtcNow,
                ProductId = slide.ProductId
            };
            await _context.Censors.AddAsync(censor);
            result = await _context.SaveChangesAsync();

            if (result > 0)
                return new ApiSuccessResult<string>("Cập nhật sản phẩm trình chiếu thành công");

            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();
            return new ApiErrorResult<string>("Cập nhật sản phâm trình chiếu thất bại");
        }
        public async Task<ApiResult<string>> ShowSlide(SlideStatusRequest request, Guid userInfoId)
        {
            var action = await _context.UserActions
                .FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.ShowSlide);

            var slide = await _context.Slides.FindAsync(request.Id);
            if (slide == null)
                return new ApiErrorResult<string>("Sản phẩm trình chiếu không tồn tại");

            slide.Status = Data.Enums.Status.Active;
            var result = await _context.SaveChangesAsync();
            if (result < 0)
                return new ApiErrorResult<string>("Hiện sản phâm trình chiếu thất bại");

            var censor = new Censor()
            {
                ActionId = action.Id,
                UserInfoId = userInfoId,
                Date = DateTime.UtcNow,
                ProductId = slide.ProductId
            };

            await _context.Censors.AddAsync(censor);
            result =await _context.SaveChangesAsync();
            if (result > 0)
                return new ApiSuccessResult<string>("Cập nhật sản phẩm trình chiếu thành công");

            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();
            return new ApiErrorResult<string>("Cập nhật sản phâm trình chiếu thất bại");
        }
        public async Task<ApiResult<List<SlideViewModel>>> GetAll()
        {
            var slides = await _context.Slides.Where(x=>x.Status == Data.Enums.Status.Active).Select(x => new SlideViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                FilePath = x.ImagePath,
                Name = x.Name,
                ProductId = x.ProductId,
                Url = x.Url,
            }).ToListAsync();
            foreach (var item in slides)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                item.ProductName = product.Name;
            }
            return new ApiSuccessResult<List<SlideViewModel>>(slides);
        }
        public async Task<ApiResult<PagedResult<SlideViewModel>>> GetAllPaging(GetSlidePagingRequest request)
        {
            var query = from s in _context.Slides
                        join p in _context.Products on s.ProductId equals p.Id
                        where s.Status == Data.Enums.Status.Active
                        select new { s, p};

            if (request.Keyword != null)
            {
                if (request.Keyword != null)
                {
                    query = query.Where(x => x.s.Name.Contains(request.Keyword) ||
                                            x.p.Name.Contains(request.Keyword));
                }
            }
            if(request.Status!=null && request.Status.HasValue)
            {
                if(request.Status==0)
                    query = query.Where(x => x.s.Status == Data.Enums.Status.InActive);
                else
                    query = query.Where(x => x.s.Status == Data.Enums.Status.Active);
            }

            int totalRow = await query.CountAsync();
            var data = await query.Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).Select(x => new SlideViewModel()
            {
                Id = x.s.Id,
                Name = x.s.Name,
                Description = x.s.Description,
                FilePath = x.s.ImagePath,
                ProductId = x.p.Id,
                Url = productUrlPattern + x.s.Id,
                ProductName = x.p.Name,
                Status = x.s.Status
            }).ToListAsync();

            List<SlideViewModel> slides = new List<SlideViewModel>();
            if (totalRow > 1)
            {
                for (int i = 0; i < data.Count - 1; i++)
                {
                    if (data.ElementAt(i).Id == data.ElementAt(i + 1).Id)
                    {
                        totalRow--;
                    }
                    else
                    {
                        slides.Add(data.ElementAt(i));
                    }
                    if (i == data.Count - 2)
                    {
                        slides.Add(data.ElementAt(i + 1));
                    }
                }
            }
            else if (totalRow == 1)
            {
                slides.Add(data.ElementAt(0));
            }

            var page = new PagedResult<SlideViewModel>()
            {
                Items = slides,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = slides.Count
            };
            return new ApiSuccessResult<PagedResult<SlideViewModel>>(page);
        }
        public async Task<ApiResult<SlideViewModel>> GetById(int slideId)
        {
            var slide = await _context.Slides.FindAsync(slideId);
            if (slide == null)
                return null;
            var slideViewModel =  new SlideViewModel()
            {
                Id = slide.Id,
                Description = slide.Description,
                FilePath = slide.ImagePath,
                Url = slide.Url,
                Name = slide.Name,
                ProductId = slide.ProductId,
                Status = slide.Status
            };

            var product = await _context.Products.FindAsync(slideViewModel.ProductId);
            slideViewModel.ProductName = product.Name;
            return new ApiSuccessResult<SlideViewModel>(slideViewModel);
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
