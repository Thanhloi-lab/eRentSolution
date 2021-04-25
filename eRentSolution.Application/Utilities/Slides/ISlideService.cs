using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.Utilities.Slides
{
    public interface ISlideService
    {
        Task<ApiResult<List<SlideViewModel>>> GetAll();
        Task<ApiResult<SlideViewModel>> GetById(int slideId);
        Task<ApiResult<string>> AddSlide(SlideCreateRequest request, Guid userInfoId);
        Task<ApiResult<string>> DeleteSlide(SlideStatusRequest request, Guid userInfoId);
        Task<ApiResult<string>> HideSlide(SlideStatusRequest request, Guid userInfoId);
        Task<ApiResult<string>> ShowSlide(SlideStatusRequest request, Guid userInfoId);
        Task<ApiResult<string>> UpdateSlide(SlideUpdateRequest request, Guid userInfoId);
        Task<ApiResult<PagedResult<SlideViewModel>>> GetAllPaging(GetSlidePagingRequest request);
    }
}
