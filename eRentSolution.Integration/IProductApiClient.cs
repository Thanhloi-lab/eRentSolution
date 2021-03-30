using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.ProductImages;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetPagings(GetProductPagingRequest request);
        Task<bool> CreateProduct(ProductCreateRequest request);
        Task<bool> UpdateProduct(ProductUpdateRequest request);
        Task<bool> DeleteProduct(int id);
        Task<ApiResult<bool>> CategoryAssign(int productId, CategoryAssignRequest request);
        Task<ProductViewModel> GetById(int productId);
        Task<PagedResult<ProductViewModel>> GetFeaturedProducts(GetProductPagingRequest request);
        Task<List<ProductViewModel>> GetLastestProducts(int take);
        Task<List<ProductImageViewModel>> GetListImages(int productId);
    }
}
