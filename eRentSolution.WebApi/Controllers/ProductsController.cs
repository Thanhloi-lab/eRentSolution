using eRentSolution.Application.Catalog.Products;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.ProductDetails;
using eRentSolution.ViewModels.Catalog.ProductImages;
using eRentSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eRentSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetProductPagingRequest request)
        {
            var product = await _productService.GetAllPaging(request);
            if (product.IsSuccessed)
                return Ok(product);
            return BadRequest(product);
        }
        [HttpGet("{userId}/GetPageProductByUserId")]
        public async Task<IActionResult> GetPageProductByUserId([FromQuery] GetProductPagingRequest request, Guid userId)
        {
            var product = await _productService.GetPageProductByUserID(request, userId);
            if (product.IsSuccessed)
                return Ok(product);
            return BadRequest(product);
        }
        [HttpGet("GetStatisticUserProduct")]
        public async Task<IActionResult> GetStatisticUserProduct([FromQuery] GetProductPagingRequest request)
        {
            var product = await _productService.GetStatisticUserProduct(request);
            if (product.IsSuccessed)
                return Ok(product);
            return BadRequest(product);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetById(id);
            if (product.IsSuccessed)
                return Ok(product);
            return BadRequest(product);
        }
        [HttpPost("{userInfoId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request, Guid userInfoId)
        {
            var result = await _productService.Create(request, userInfoId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        
        [HttpPut("{userInfoId}/{productId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request, Guid userInfoId, [FromRoute]int productId)
        {
            request.Id = productId;
            var result = await _productService.Update(request, userInfoId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{userInfoId}/createfeature/{productId}")]
        public async Task<IActionResult> CreateFeature(Guid userInfoId, int productId)
        {
            var result = await _productService.CreateFeature(productId, userInfoId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{userInfoId}/deletefeature/{productId}")]
        public async Task<IActionResult> DeleteFeature(Guid userInfoId, int productId)
        {
            var result = await _productService.DeleteFeature(productId, userInfoId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("viewcount/{productId}")]
        public async Task<IActionResult> AddViewcount(int productId)
        {
            var result = await _productService.AddViewcount(productId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("hide/{userInfoId}/{id}")]
        public async Task<IActionResult> Hide(int id, Guid userInfoId)
        {
            var result = await _productService.Hide(id, userInfoId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("show/{userInfoId}/{id}")]
        public async Task<IActionResult> Show(int id, Guid userInfoId)
        {
            var result = await _productService.Show(id, userInfoId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("active/{userInfoId}/{id}")]
        public async Task<IActionResult> ActiveProduct(int id, Guid userInfoId)
        {
            var result = await _productService.ActiveProduct(id, userInfoId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("inactive/{userInfoId}/{id}")]
        public async Task<IActionResult> InActiveProduct(int id, Guid userInfoId)
        {
            var result = await _productService.InActiveProduct(id, userInfoId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{id}/categories")]
        public async Task<IActionResult> CategoryAssign(int id, [FromBody] CategoryAssignRequest request)
        {
            var result = await _productService.CategoryAssign(id, request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("feature")]
        public async Task<IActionResult> GetFeaturedProducts([FromQuery] GetProductPagingRequest request)
        {
            var result = await _productService.GetFeaturedProducts(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("lastest/{take}")]
        public async Task<IActionResult> GetLastestProducts(int take)
        {
            var result = await _productService.GetLastestProducts(take);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("isMyProduct/{userId}/{productId}")]
        public async Task<IActionResult> IsMyProduct(int productId, Guid userId)
        {
            var result = await _productService.IsMyProduct(userId, productId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("updateDetail/{userId}")]
        public async Task<IActionResult> UpdateDetail([FromBody] ProductDetailUpdateRequest request, Guid userId)
        {
            var result = await _productService.UpdateDetail(request, userId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("addProductDetail/{userInfoId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProductDetail([FromForm] ProductDetailCreateRequest request, Guid userInfoId)
        {
            var result = await _productService.AddDetail(request, userInfoId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("productDetail/{id}")]
        public async Task<IActionResult> GetProductDetailById(int id)
        {
            var result = await _productService.GetProductDetailById(id);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("{userId}/deleteDetail/{productDetailId}")]
        public async Task<IActionResult> DeleteDetail(int productDetailId, Guid userId)
        {
            var result = await _productService.DeleteDetail(productDetailId, userId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("price/{userInfoId}")]
        public async Task<IActionResult> UpdatePrice([FromForm] ProductUpdatePriceRequest request, Guid userInfoId)
        {
            var result = await _productService.UpdatePrice(request.ProductDetailId, request.NewPrice, userInfoId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("stock/{userInfoId}")]
        public async Task<IActionResult> UpdateStock([FromForm] ProductUpdateStockRequest request, Guid userInfoId)
        {
            var result = await _productService.UpdateStock(request.ProductDetailId, request.AddedQuantity, userInfoId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }





        // ============== IMAGE===========
        [HttpGet("img/{imageId}")]
        public async Task<IActionResult> GetImageById(int imageId)
        {
            var result = await _productService.GetImageById(imageId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        } 
        [HttpGet("imgs/{productId}")]
        public async Task<IActionResult> GetImageByProductId(int productId)
        {
            var result = await _productService.GetListImage(productId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("add-img/{userId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddImage([FromForm] ProductImageCreateRequest request, Guid userId)
        {
            var result = await _productService.AddImage(request, userId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("{userId}/img/{imageId}")]
        public async Task<IActionResult> RemoveImage(int imageId, Guid userId)
        {
            var result = await _productService.DeleteImage(imageId, userId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("update-img/{userId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage([FromForm] ProductImageUpdateRequest request, Guid userId)
        {
            var result = await _productService.UpdateImage(request, userId);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
