using eRentSolution.Data.EF;
using eRentSolution.ViewModels.Catalog.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly eRentDbContext _context;

        public CategoryService(eRentDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryViewModel>> GetAll()
        {
            return await _context.Categories.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId
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
                ParentId = x.c.ParentId
            }).ToListAsync();
        }

        public async Task<CategoryViewModel> GetById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category != null)
                return null;
            return new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId
            };
        }
    }
}
