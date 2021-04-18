using eRentSolution.Application.Catalog.Products;
using eRentSolution.ViewModels.Catalog.Categories;
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
            return Ok(product);
        }
        [HttpGet("{userId}/GetPageProductByUserId")]
        public async Task<IActionResult> GetPageProductByUserId([FromQuery] GetProductPagingRequest request, Guid userId)
        {
            var product = await _productService.GetPageProductByUserID(request, userId);
            return Ok(product);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return BadRequest("Cannot find product");
            }
            return Ok(product);
        }
        [HttpPost("{userInfoId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Creat([FromForm] ProductCreateRequest request, Guid userInfoId)
        {
            var productId = await _productService.Create(request, userInfoId);
            if (productId == 0)
            {
                return BadRequest();
            }
            var product = await _productService.GetById(productId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }
        [HttpPut("{userInfoId}/{productId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request, Guid userInfoId, [FromRoute]int productId)
        {
            request.Id = productId;
            var isSuccessful = await _productService.Update(request, userInfoId);
            if (isSuccessful == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("{userInfoId}/createfeature/{productId}")]
        public async Task<IActionResult> CreateFeature(Guid userInfoId, int productId)
        {
            var result = await _productService.CreateFeature(productId, userInfoId);
            if (result.IsSuccessed == false)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPut("{userInfoId}/deletefeature/{productId}")]
        public async Task<IActionResult> DeleteFeature(Guid userInfoId, int productId)
        {
            var result = await _productService.DeleteFeature(productId, userInfoId);
            if (result.IsSuccessed == false)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPut("{productId}")]
        public async Task<IActionResult> AddViewcount(int productId)
        {
            var result = await _productService.AddViewcount(productId);
            if (result == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccessful = await _productService.Delete(id);
            if (isSuccessful == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("hide/{userInfoId}/{id}")]
        public async Task<IActionResult> Hide(int id, Guid userInfoId)
        {
            var isSuccessful = await _productService.Hide(id, userInfoId);
            if (isSuccessful == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("show/{userInfoId}/{id}")]
        public async Task<IActionResult> Show(int id, Guid userInfoId)
        {
            var isSuccessful = await _productService.Show(id, userInfoId);
            if (isSuccessful == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("price/{userInfoId}/{productDetailId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productDetailId, decimal newPrice, Guid userInfoId)
        {
            var isSuccessful = await _productService.UpdatePrice(productDetailId, newPrice, userInfoId);
            if (isSuccessful == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("stock/{userInfoId}/{productDetailId}/{addedQuantity}")]
        public async Task<IActionResult> UpdateStock(int productDetailId, int addedQuantity, Guid userInfoId)
        {
            var isSuccessful = await _productService.UpdateStock(productDetailId, addedQuantity, userInfoId);
            if (isSuccessful == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("{id}/categories")]
        public async Task<IActionResult> CategoryAssign(int id, [FromBody] CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.CategoryAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("feature")]
        public async Task<IActionResult> GetFeaturedProducts([FromQuery] GetProductPagingRequest request)
        {
            var products = await _productService.GetFeaturedProducts(request);
            return Ok(products);
        }
        [HttpGet("lastest/{take}")]
        public async Task<IActionResult> GetLastestProducts(int take)
        {
            var products = await _productService.GetLastestProducts(take);
            return Ok(products);
        }
        [HttpGet("isMyProduct/{userId}/{productId}")]
        public async Task<IActionResult> IsMyProduct(int productId, Guid userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.IsMyProduct(userId, productId);
            return Ok(result);
        }

        // ============== IMAGE===========
        [HttpGet("img/{imageId}")]
        public async Task<IActionResult> GetImageById(int imageId)
        {
            var Image = await _productService.GetImageById(imageId);
            if (Image == null)
            {
                return BadRequest("Cannot find Image");
            }
            return Ok(Image);
        }
        [HttpGet("imgs/{productId}")]
        public async Task<IActionResult> GetImageByProductId(int productId)
        {
            var Image = await _productService.GetListImage(productId);
            if (Image == null)
            {
                return BadRequest("Cannot find Image");
            }
            return Ok(Image);
        }

        [HttpPost("add-img")]
        public async Task<IActionResult> AddImages([FromForm] ProductImageCreateRequest request)
        {
            //     var productId = await _productService.Create(request);
            var imageId = await _productService.AddImages(request);
            if (imageId == 0)
            {
                return BadRequest();
            }
            var image = await _productService.GetImageById(request.ProductId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }
        [HttpDelete("img/{imageID}")]
        public async Task<IActionResult> RemoveImages(int imageID)
        {
            var result = await _productService.RemoveImages(imageID);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("update-img")]
        public async Task<IActionResult> UpdateImages([FromForm] ProductImageUpdateRequest request)
        {
            var result = await _productService.UpdateImages(request);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
}
