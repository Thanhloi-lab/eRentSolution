using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.ProductDetails;
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
        #region ------ PRODUCT ACTION --------
        Task<bool> CreateProduct(ProductCreateRequest request, Guid userInfoId, string tokenName);
        Task<bool> UpdateProduct(ProductUpdateRequest request, Guid userInfoId, string tokenName);
        Task<bool> HideProduct(int productId, Guid userInfoId, string tokenName);
        Task<bool> ShowProduct(int productId, Guid userInfoId, string tokenName);
        Task<bool> DeleteProduct(int id, string tokenName);
        Task<bool> CreateFeature(FeatureProductRequest request, string tokenName, Guid userInfoId);
        Task<bool> DeleteFeature(FeatureProductRequest request, string tokenName, Guid userInfoId);
        Task<ApiResult<bool>> CategoryAssign(int productId, CategoryAssignRequest request, string tokenName);
        Task<bool> IsMyProduct(int productId, Guid userId, string tokenName);
        #endregion

        #region -------GET AND CHECK PRODUCT ---------
        Task<PagedResult<ProductViewModel>> GetPagings(GetProductPagingRequest request, string tokenName);
        Task<PagedResult<ProductViewModel>> GetFeaturedProducts(GetProductPagingRequest request, string tokenName);
        Task<List<ProductViewModel>> GetLastestProducts(int take, string tokenName);
        Task<PagedResult<ProductViewModel>> GetPageProductsByUserId(GetProductPagingRequest request, Guid userId, string tokenName);
        Task<ProductViewModel> GetById(int productId, string tokenName);
        
        #endregion

        #region ------PRODUCT DETAIL-------
        Task<bool> AddProductDetail(ProductDetailCreateRequest request, Guid userInfoId, string tokenName);
        Task<bool> UpdateDetail(ProductDetailUpdateRequest request, Guid userId, string tokenName);
        Task<ApiResult<bool>> UpdateStock(ProductUpdateStockRequest request, Guid userInfoId, string tokenName);
        Task<ApiResult<bool>> UpdatePrice(ProductUpdatePriceRequest request, Guid userInfoId, string tokenName);
        Task<ApiResult<bool>> AddViewcount(int productId, string tokenName);
        Task<bool> DeleteDetail(int productDetailId, Guid userId, string tokenName);
        Task<ProductDetailViewModel> GetProductDetailById(int productDetailId, string tokenName);
        Task<bool> HideProductDetail(int productDetailId, Guid userInfoId, string tokenName);
        Task<bool> ShowProductDetail(int productDetailId, Guid userInfoId, string tokenName);
        #endregion

        #region ------IMAGE------
        Task<List<ProductImageViewModel>> GetListImages(int productId, string tokenName);
        Task<ProductImageViewModel> GetImageById(int imageId, string tokenName);
        Task<ApiResult<string>> UpdateImage(ProductImageUpdateRequest request, string tokenName, Guid userId);
        Task<ApiResult<string>> AddImage(ProductImageCreateRequest request, string tokenName, Guid userId);
        Task<bool> DeleteImage(int imageId, Guid userId, string tokenName);
        #endregion
    }
}
