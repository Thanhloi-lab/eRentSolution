using eRentSolution.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public class CategoryApiClient : BaseApiClient ,ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory, 
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<List<CategoryViewModel>> GetAll()
        {
            var result = await GetListAsync<CategoryViewModel>($"api/categories");
            return result;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryByProductId(int productId)
        {
            var result = await GetListAsync<CategoryViewModel>($"api/categories/productcategories");
            return result;
        }

        public async Task<CategoryViewModel> GetById(int id)
        {
            var result = await GetAsync<CategoryViewModel>($"api/categories/{id}");
            return result;
        }
    }
}
