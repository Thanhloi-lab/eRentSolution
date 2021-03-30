using eRentSolution.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryViewModel>> GetAll();
        Task<CategoryViewModel> GetById(int id);
        Task<List<CategoryViewModel>> GetAllCategoryByProductId(int productId);
    }
}
