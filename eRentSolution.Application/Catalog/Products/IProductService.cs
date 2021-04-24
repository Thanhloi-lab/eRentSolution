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
        Task<ApiResult<string>> AddImage( ProductImageCreateRequest request, Guid userId);
        Task<bool> DeleteImage(int imageId, Guid userId);
        Task<bool> DeleteDetail(int productDetailId, Guid userId);
        Task<ApiResult<string>> UpdateImage( ProductImageUpdateRequest request, Guid userId);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
        Task<ProductImageViewModel> GetImageById(int imageId);
        Task<bool> IsMyProduct(Guid userId, int productId);
        Task<ApiResult<bool>> UpdateDetail(ProductDetailUpdateRequest request, Guid userInfoId);
        Task<int> AddDetail(ProductDetailCreateRequest request, Guid userInfoId);
        Task<ProductDetailViewModel> GetProductDetailById(int productDetailId);
        //Task<PagedResult<ProductViewModel>> GetAllPagingByCategoryId(GetProductPagingByCategoryIdRequest request);
    }
}
