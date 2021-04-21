using eRentSolution.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryViewModel>> GetAll(string tokenName);
        Task<CategoryViewModel> GetById(int id, string tokenName);
        Task<List<CategoryViewModel>> GetAllCategoryByProductId(int productId, string tokenName);
        Task<bool> UpdateImage(CategoryImageUpdateRequest request, string tokenName);
    }
}
