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
        Task<ApiResult<int>> CreateProduct(ProductCreateRequest request, Guid userInfoId, string tokenName);
        Task<ApiResult<string>> UpdateProduct(ProductUpdateRequest request, Guid userInfoId, string tokenName);
        Task<ApiResult<string>> HideProduct(int productId, Guid userInfoId, string tokenName);
        Task<ApiResult<string>> ShowProduct(int productId, Guid userInfoId, string tokenName);
        Task<ApiResult<string>> ActiveProduct(int productId, Guid userInfoId, string tokenName);
        Task<ApiResult<string>> InActiveProduct(int productId, Guid userInfoId, string tokenName);
        Task<ApiResult<string>> DeleteProduct(int id, string tokenName);
        Task<ApiResult<string>> CreateFeature(FeatureProductRequest request, string tokenName, Guid userInfoId);
        Task<ApiResult<string>> DeleteFeature(FeatureProductRequest request, string tokenName, Guid userInfoId);
        Task<ApiResult<string>> CategoryAssign(int productId, CategoryAssignRequest request, string tokenName);
        Task<ApiResult<string>> IsMyProduct(int productId, Guid userId, string tokenName);
        #endregion

        #region -------GET AND CHECK PRODUCT ---------
        Task<ApiResult<PagedResult<ProductViewModel>>> GetPagings(GetProductPagingRequest request, string tokenName);
        Task<ApiResult<PagedResult<ProductViewModel>>> GetFeaturedProducts(GetProductPagingRequest request, string tokenName);
        Task<ApiResult<List<ProductViewModel>>> GetLastestProducts(int take, string tokenName);
        Task<ApiResult<PagedResult<ProductViewModel>>> GetPageProductsByUserId(GetProductPagingRequest request, Guid userId, string tokenName);
        Task<ApiResult<PagedResult<UserProductStatisticViewModel>>> GetStatisticUserProduct(GetProductPagingRequest request, string tokenName);
        Task<ApiResult<ProductViewModel>> GetById(int productId, string tokenName);

        #endregion

        #region ------PRODUCT DETAIL-------
        Task<ApiResult<string>> AddProductDetail(ProductDetailCreateRequest request, Guid userInfoId, string tokenName);
        Task<ApiResult<string>> UpdateDetail(ProductDetailUpdateRequest request, Guid userId, string tokenName);
        Task<ApiResult<string>> UpdateStock(ProductUpdateStockRequest request, Guid userInfoId, string tokenName);
        Task<ApiResult<string>> UpdatePrice(ProductUpdatePriceRequest request, Guid userInfoId, string tokenName);
        Task<ApiResult<string>> AddViewcount(int productId, string tokenName);
        Task<ApiResult<string>> DeleteDetail(int productDetailId, Guid userId, string tokenName);
        Task<ApiResult<string>> HideProductDetail(int productDetailId, Guid userInfoId, string tokenName);
        Task<ApiResult<string>> ShowProductDetail(int productDetailId, Guid userInfoId, string tokenName);
        Task<ApiResult<ProductDetailViewModel>> GetProductDetailById(int productDetailId, string tokenName);
        #endregion

        #region ------IMAGE------
        Task<ApiResult<List<ProductImageViewModel>>> GetListImages(int productId, string tokenName);
        Task<ApiResult<ProductImageViewModel>> GetImageById(int imageId, string tokenName);
        Task<ApiResult<string>> UpdateImage(ProductImageUpdateRequest request, string tokenName, Guid userId);
        Task<ApiResult<string>> AddImage(ProductImageCreateRequest request, string tokenName, Guid userId);
        Task<ApiResult<string>> DeleteImage(int imageId, Guid userId, string tokenName);
        #endregion
    }
}
