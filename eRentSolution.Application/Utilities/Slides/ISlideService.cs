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
        Task<List<SlideViewModel>> GetAll();
        Task<SlideViewModel> GetById(int slideId);
        Task<int> AddSlide(SlideCreateRequest request, Guid userInfoId);
        Task<bool> DeleteSlide(SlideStatusRequest request, Guid userInfoId);
        Task<bool> HideSlide(SlideStatusRequest request, Guid userInfoId);
        Task<bool> ShowSlide(SlideStatusRequest request, Guid userInfoId);
        Task<bool> UpdateSlide(SlideUpdateRequest request, Guid userInfoId);
        Task<PagedResult<SlideViewModel>> GetAllPaging(GetSlidePagingRequest request);
    }
}
