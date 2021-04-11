using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.ProductImages;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetPagings(GetProductPagingRequest request, string tokenName);
        Task<bool> CreateProduct(ProductCreateRequest request, Guid userInfoId, string tokenName);
        Task<bool> UpdateProduct(ProductUpdateRequest request, Guid userInfoId, string tokenName);
        Task<bool> DeleteProduct(int id, Guid userInfoId, string tokenName);
        Task<ApiResult<bool>> CategoryAssign(int productId, CategoryAssignRequest request, string tokenName);
        Task<ProductViewModel> GetById(int productId, string tokenName);
        Task<PagedResult<ProductViewModel>> GetFeaturedProducts(GetProductPagingRequest request, string tokenName);
        Task<List<ProductViewModel>> GetLastestProducts(int take, string tokenName);
        Task<List<ProductImageViewModel>> GetListImages(int productId, string tokenName);
    }
}
