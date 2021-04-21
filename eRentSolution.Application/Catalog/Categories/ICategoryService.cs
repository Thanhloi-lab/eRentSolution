using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAll();
        Task<CategoryViewModel> GetById(int id);
        Task<List<CategoryViewModel>> GetAllCategoryByProductId(int productId);
        Task<bool> UpdateImage(CategoryImageUpdateRequest request);
        Task<PagedResult<CategoryViewModel>> GetAllPaging(GetCategoryPagingRequest request);
    }
}
