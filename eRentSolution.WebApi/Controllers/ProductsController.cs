using eRentSolution.Application.Catalog.Products;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.ProductImages;
using eRentSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //[HttpGet("GetAllByCategoryId")]
        //public async Task<IActionResult> Get([FromQuery] GetProductPagingByCategoryIdRequest request)
        //{
        //    var product = await _productService.GetAllPagingByCategoryId(request);
        //    return Ok(product);
        //}
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetProductPagingRequest request)
        {
            var product = await _productService.GetAllPaging(request);
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
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Creat([FromForm] ProductCreateRequest request)
        {
            var productId = await _productService.Create(request);
            if (productId == 0)
            {
                return BadRequest();
            }
            var product = await _productService.GetById(productId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            var isSuccessful = await _productService.Update(request);
            if (isSuccessful == false)
            {
                return BadRequest();
            }
            return Ok();
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccessful = await _productService.Delete(id);
            if (isSuccessful == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("price/{id}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int id, decimal newPrice)
        {
            var isSuccessful = await _productService.UpdatePrice(id, newPrice);
            if (isSuccessful == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("stock/{id}/{addedQuantity}")]
        public async Task<IActionResult> UpdateStock(int id, int addedQuantity)
        {
            var isSuccessful = await _productService.UpdateStock(id, addedQuantity);
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
        public async Task<IActionResult> GetImageByProductId(int imageId)
        {
            var Image = await _productService.GetImageById(imageId);
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
