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
        Task<ApiResult<List<SlideViewModel>>> GetAll(string tokenName);
        Task<ApiResult<SlideViewModel>> GetById(int slideId, string tokenName);
        Task<ApiResult<string>> DeleteSlide(SlideStatusRequest request, string tokenName, Guid userInfoId);
        Task<ApiResult<string>> HideSlide(SlideStatusRequest request, string tokenName, Guid userInfoId);
        Task<ApiResult<string>> ShowSlide(SlideStatusRequest request, string tokenName, Guid userInfoId);
        Task<ApiResult<string>> UpdateSlide(SlideUpdateRequest request, string tokenName, Guid userInfoId);
        Task<ApiResult<string>> CreateSlide(SlideCreateRequest request, string tokenName, Guid userInfoId);
        Task<ApiResult<PagedResult<SlideViewModel>>> GetPagings(GetSlidePagingRequest request, string tokenName);
    }
}
