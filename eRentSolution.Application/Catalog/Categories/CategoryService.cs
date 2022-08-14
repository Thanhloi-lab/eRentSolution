using eRentSolution.Application.Common;
using eRentSolution.Data.EF;
using eRentSolution.Data.Entities;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly eRentDbContext _context;
        private readonly IStorageService _storageService;

        public CategoryService(eRentDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<string>> CreateCategory(CategoryCreateRequest request)
        {
           
            var category = new Category()
            {
                Name = request.CategoryName,
                ParentId = request.ParentId,
                DateCreate = DateTime.UtcNow
            };
            if (request.ImageFile != null)
            {
                category.ImagePath = await this.SaveFile(request.ImageFile);
                category.ImageSize = request.ImageFile.Length;
            }
            else
            {
                return new ApiErrorResult<string>("Ảnh không tồn tại");
            }
            await _context.Categories.AddAsync(category);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result < 1)
            {
                _storageService.DeleteFile(category.ImagePath);
                return new ApiErrorResult<string>("Thêm danh mục thất bại");
            }
            return new ApiSuccessResult<string>("Tạo danh mục thành công");
        }

        public async Task<ApiResult<string>> DeleteCategory(int categoryId)
        {
            var query = from c in _context.Categories
                        join pic in _context.NewsInCategories on c.Id equals pic.CategoryId
                        where c.Id == categoryId
                        select new { c };

            if(query.Count()>0)
            {
                return new ApiErrorResult<string>("Không thể xóa danh mục đã có sản phẩm");
            }
            var category = await _context.Categories.FindAsync(categoryId);
            _context.Categories.Remove(category);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result < 1)
            {
                return new ApiErrorResult<string>("Xóa danh mục thất bại");
            }
            _storageService.DeleteFile(category.ImagePath);
            return new ApiSuccessResult<string>("Xóa danh mục thành công");
        }

        public async Task<ApiResult<List<CategoryViewModel>>> GetAll()
        {
            var categories =  await _context.Categories.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId,
                Image = x.ImagePath
            }).ToListAsync();
            if (categories != null)
                return new ApiSuccessResult<List<CategoryViewModel>>(categories);
            return new ApiErrorResult<List<CategoryViewModel>>("Không tìm thấy danh mục nào");
        }

        public async Task<ApiResult<List<CategoryViewModel>>> GetAllCategoryByProductId(int productId)
        {
            var query = from c in _context.Categories
                        join pic in _context.NewsInCategories on c.Id equals pic.CategoryId
                        join p in _context.News on pic.NewsId equals p.Id
                        where p.Id == productId
                        select new { c };
            if(query.Count()<0)
            {
                return new ApiErrorResult<List<CategoryViewModel>>("Không tìm thấy danh mục nào");
            }    
            var categories = await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                ParentId = x.c.ParentId,
                Image = x.c.ImagePath
            }).ToListAsync();
            return new ApiSuccessResult<List<CategoryViewModel>>(categories);
        }

        public async Task<ApiResult<PagedResult<CategoryViewModel>>> GetAllPaging(GetCategoryPagingRequest request)
        {
            var query = from c in _context.Categories
                        select new { c };
            if (request.Keyword != null)
            {
                query = query.Where(x => x.c.Name.Contains(request.Keyword));
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Image = x.c.ImagePath,
                ParentId = x.c.ParentId == null ? -1 : x.c.ParentId
            }).ToListAsync();

            var page = new PagedResult<CategoryViewModel>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = data.Count
            };
            return new ApiSuccessResult<PagedResult<CategoryViewModel>>(page);
        }

        public async Task<ApiResult<CategoryViewModel>> GetById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return new ApiErrorResult<CategoryViewModel>("Không tìm thấy danh mục nào");
            var viewModel = new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                Image = category.ImagePath
            };
            return new ApiSuccessResult<CategoryViewModel>(viewModel);
        }

        public async Task<ApiResult<string>> UpdateCategory(CategoryUpdateRequest request)
        {
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if(category==null)
                return new ApiErrorResult<string>("Danh mục không tồn tại");

            var query = from c in _context.Categories
                        join pic in _context.NewsInCategories on c.Id equals pic.CategoryId
                        where c.Id == request.CategoryId
                        select new { c };

            if (query.Count() > 0)
            {
                return new ApiErrorResult<string>("Không thể sửa danh mục đã có sản phẩm");
            }

            category.Name = request.CategoryName;

            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
            {
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<string>("Chỉnh sửa danh mục thành công");
            }

            return new ApiErrorResult<string>("Chỉnh sửa danh mục không thành công, vui lòng thử lại sau");
        }

        public async Task<ApiResult<string>> UpdateImage(CategoryImageUpdateRequest request)
        {
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                return new ApiErrorResult<string>("Danh mục không tồn tại");
            int isDeleteSuccess = 0;
            if (request.ImageFile != null)
            {
                if (category.ImagePath != SystemConstant.DefaultAvatar && category.ImagePath != null)
                    isDeleteSuccess = _storageService.DeleteFile(category.ImagePath);
                if (isDeleteSuccess == -1)
                    return new ApiErrorResult<string>("Chỉnh sửa ảnh không thành công, vui lòng thử lại sau");
                category.ImagePath = await this.SaveFile(request.ImageFile);
                category.ImageSize = request.ImageFile.Length;
            }

            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }

            if (result > 0)
            {
                return new ApiSuccessResult<string>("Chỉnh sửa ảnh thành công");
            }
            _storageService.DeleteFile(category.ImagePath);
            return new ApiErrorResult<string>("Chỉnh sửa ảnh không thành công, vui lòng thử lại sau");
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
