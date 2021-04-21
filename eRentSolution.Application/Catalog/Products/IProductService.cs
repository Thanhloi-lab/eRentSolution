using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.ProductDetails;
using eRentSolution.ViewModels.Catalog.ProductImages;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<int> Create(ProductCreateRequest request, Guid userInfoId);
        Task<bool> Update(ProductUpdateRequest request, Guid userInfoId);
        Task<bool> UpdateDetail(ProductDetailUpdateRequest request, Guid userInfoId);
        Task<bool> Hide(int productId, Guid userInfoId);
        Task<bool> Show(int productId, Guid userInfoId);
        Task<bool> Delete(int productId);
        Task<bool> UpdatePrice(int productId, decimal newPrice, Guid userInfoId);
        Task<bool> UpdateStock(int productId, int addedQuantity, Guid userInfoId);
        Task<ApiResult<bool>> CreateFeature(int productId, Guid userInfoId);
        Task<ApiResult<bool>> DeleteFeature(int productId, Guid userInfoId);
        Task<bool> AddViewcount(int productId);
        Task<ProductViewModel> GetById(int id);
        Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<PagedResult<ProductViewModel>> GetFeaturedProducts(GetProductPagingRequest request);
        Task<PagedResult<ProductViewModel>> GetPageProductByUserID(GetProductPagingRequest request, Guid userId);
        Task<List<ProductViewModel>> GetLastestProducts( int take);
        Task<int> AddImage( ProductImageCreateRequest request, int productDetailId);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImage( ProductImageUpdateRequest request);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
        Task<ProductImageViewModel> GetImageById(int imageId);
        Task<bool> IsMyProduct(Guid userId, int productId);
        //Task<PagedResult<ProductViewModel>> GetAllPagingByCategoryId(GetProductPagingByCategoryIdRequest request);
    }
}
