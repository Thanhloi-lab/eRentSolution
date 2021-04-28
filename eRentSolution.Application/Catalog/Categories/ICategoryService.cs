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
        Task<ApiResult<List<CategoryViewModel>>> GetAll();
        Task<ApiResult<CategoryViewModel>> GetById(int id);
        Task<ApiResult<List<CategoryViewModel>>> GetAllCategoryByProductId(int productId);
        Task<ApiResult<string>> UpdateImage(CategoryImageUpdateRequest request);
        Task<ApiResult<string>> DeleteCategory(int categoryId);
        Task<ApiResult<string>> CreateCategory(CategoryCreateRequest request);
        Task<ApiResult<string>> UpdateCategory(CategoryUpdateRequest request);
        Task<ApiResult<PagedResult<CategoryViewModel>>> GetAllPaging(GetCategoryPagingRequest request);
    }
}
