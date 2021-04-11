using eRentSolution.ViewModels.Catalog.Categories;
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
        Task<bool> Update(ProductUpdateRequest request, Guid userInfoId, int productId);
        Task<bool> Delete(int productId, Guid userInfoId);
        Task<bool> UpdatePrice(int productId, decimal newPrice, Guid userInfoId);
        Task<bool> UpdateStock(int productId, int addedQuantity, Guid userInfoId);
        Task<bool> AddViewcount(int productId);
        Task<ProductViewModel> GetById(int id);
        Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<PagedResult<ProductViewModel>> GetFeaturedProducts(GetProductPagingRequest request);
        Task<List<ProductViewModel>> GetLastestProducts( int take);
        Task<int> AddImages( ProductImageCreateRequest request);
        Task<int> RemoveImages(int imageId);
        Task<int> UpdateImages( ProductImageUpdateRequest request);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
        Task<ProductImageViewModel> GetImageById(int imageId);
        //Task<PagedResult<ProductViewModel>> GetAllPagingByCategoryId(GetProductPagingByCategoryIdRequest request);
    }
}
