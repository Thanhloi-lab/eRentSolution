using eRentSolution.Application.Common;
using eRentSolution.Data.EF;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Catalog.Categories;
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

        public async Task<List<CategoryViewModel>> GetAll()
        {
            return await _context.Categories.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId,
                 Image = x.ImagePath
            }).ToListAsync();
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryByProductId(int productId)
        {
            var query = from c in _context.Categories
                        join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                        join p in _context.Products on pic.ProductId equals p.Id
                        where p.Id == productId
                        select new { c };
            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                ParentId = x.c.ParentId, 
                Image=x.c.ImagePath
            }).ToListAsync();
        }

        public async Task<CategoryViewModel> GetById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return null;
            return new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                Image = category.ImagePath
            };
        }

        public async Task<bool> UpdateImage(CategoryImageUpdateRequest request)
        {
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if(category!=null)
            {
                if(request.ImageFile!=null)
                {
                    if (category.ImagePath != SystemConstant.DefaultCategory)
                        await _storageService.DeleteFileAsync(category.ImagePath);
                    category.ImagePath = await this.SaveFile(request.ImageFile);
                }
            }
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;
            return false;
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
