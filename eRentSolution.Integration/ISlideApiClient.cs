using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public interface ISlideApiClient
    {
        Task<List<SlideViewModel>> GetAll(string tokenName);
        Task<SlideViewModel> GetById(int slideId, string tokenName);
        Task<bool> DeleteSlide(SlideStatusRequest request, string tokenName, Guid userInfoId);
        Task<bool> HideSlide(SlideStatusRequest request, string tokenName, Guid userInfoId);
        Task<bool> ShowSlide(SlideStatusRequest request, string tokenName, Guid userInfoId);
        Task<bool> UpdateSlide(SlideUpdateRequest request, string tokenName, Guid userInfoId);
        Task<bool> CreateSlide(SlideCreateRequest request, string tokenName, Guid userInfoId);
        Task<PagedResult<SlideViewModel>> GetPagings(GetSlidePagingRequest request, string tokenName);
    }
}
