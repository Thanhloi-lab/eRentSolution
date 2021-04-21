using eRentSolution.Application.Utilities.Slides;
using eRentSolution.Utilities.Constants;
using eRentSolution.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eRentSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidesController : ControllerBase
    {
        private readonly ISlideService _slideService;

        public SlidesController(ISlideService slideService)
        {
            _slideService = slideService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetSlidePagingRequest request)
        {
            var product = await _slideService.GetAllPaging(request);
            return Ok(product);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var slides = await _slideService.GetAll();
            return Ok(slides);
        }
        [HttpGet("{slideId}")]
        public async Task<IActionResult> GetById(int slideId)
        {
            var slide = await _slideService.GetById(slideId);
            return Ok(slide);
        }
        [HttpPost("{userInfoId}/create/{productId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(Guid userInfoId, int productId, [FromForm]SlideCreateRequest request)
        {
            request.ProductId = productId;
            var result = await _slideService.AddSlide(request, userInfoId);
            return Ok(result);
        }
        [HttpPut("{userInfoId}/update/{slideId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(Guid userInfoId, int slideId, [FromForm] SlideUpdateRequest request)
        {
            request.Id = slideId;
            var result = await _slideService.UpdateSlide(request, userInfoId);
            return Ok(result);
        }
        [HttpDelete("{userInfoId}/delete/{slideId}")]
        public async Task<IActionResult> Delete(int slideId, Guid userInfoId, [FromForm] SlideStatusRequest request)
        {
            request.Id = slideId;
            var result = await _slideService.DeleteSlide(request, userInfoId);
            return Ok(result);
        }
        [HttpDelete("{userInfoId}/hide/{slideId}")]
        public async Task<IActionResult> Hide(Guid userInfoId, int slideId, [FromForm] SlideStatusRequest request)
        {
            request.Id = slideId;
            var result = await _slideService.HideSlide(request, userInfoId);
            return Ok(result);
        }
        [HttpDelete("{userInfoId}/show/{slideId}")]
        public async Task<IActionResult> Show(Guid userInfoId, int slideId, [FromForm] SlideStatusRequest request)
        {
            request.Id = slideId;
            var result = await _slideService.ShowSlide(request, userInfoId);
            return Ok(result);
        }
    }
}
