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
        Task<ApiResult<int>> Create(ProductCreateRequest request, Guid userInfoId);
        Task<ApiResult<string>> Update(ProductUpdateRequest request, Guid userInfoId);
        Task<ApiResult<string>> Hide(int productId, Guid userInfoId);
        Task<ApiResult<string>> Show(int productId, Guid userInfoId);
        Task<ApiResult<string>> InActiveProduct(int productId, Guid userInfoId);
        Task<ApiResult<string>> ActiveProduct(int productId, Guid userInfoId);
        Task<ApiResult<string>> Delete(int productId);
        Task<ApiResult<string>> UpdatePrice(int productId, decimal newPrice, Guid userInfoId);
        Task<ApiResult<string>> UpdateStock(int productId, int addedQuantity, Guid userInfoId);
        Task<ApiResult<string>> CreateFeature(int productId, Guid userInfoId);
        Task<ApiResult<string>> DeleteFeature(int productId, Guid userInfoId);
        Task<ApiResult<string>> AddViewcount(int productId);
        Task<ApiResult<string>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<ApiResult<string>> AddImage( ProductImageCreateRequest request, Guid userId);
        Task<ApiResult<string>> DeleteImage(int imageId, Guid userId);
        Task<ApiResult<string>> DeleteDetail(int productDetailId, Guid userId);
        Task<ApiResult<string>> UpdateImage( ProductImageUpdateRequest request, Guid userId);
        Task<ApiResult<string>> IsMyProduct(Guid userId, int productId);
        Task<ApiResult<string>> UpdateDetail(ProductDetailUpdateRequest request, Guid userInfoId);
        Task<ApiResult<string>> AddDetail(ProductDetailCreateRequest request, Guid userInfoId);


        Task<ApiResult<ProductViewModel>> GetById(int id);
        Task<ApiResult<ProductImageViewModel>> GetImageById(int imageId);
        Task<ApiResult<List<ProductViewModel>>> GetLastestProducts(int take);
        Task<ApiResult<List<ProductImageViewModel>>> GetListImage(int productId);
        Task<ApiResult<ProductDetailViewModel>> GetProductDetailById(int productDetailId);
        Task<ApiResult<PagedResult<ProductViewModel>>> GetAllPaging(GetProductPagingRequest request);
        Task<ApiResult<PagedResult<ProductViewModel>>> GetFeaturedProducts(GetProductPagingRequest request);
        Task<ApiResult<PagedResult<ProductViewModel>>> GetPageProductByUserID(GetProductPagingRequest request, Guid userId);
        Task<ApiResult<PagedResult<UserProductStatisticViewModel>>> GetStatisticUserProduct(GetProductPagingRequest request);

        //Task<PagedResult<ProductViewModel>> GetAllPagingByCategoryId(GetProductPagingByCategoryIdRequest request);
    }
}
