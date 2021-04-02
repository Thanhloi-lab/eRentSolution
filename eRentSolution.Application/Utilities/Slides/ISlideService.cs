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
        Task<bool> AddSlide(int productId);
        Task<bool> DeleteSlide(int slideId);
        Task<bool> UpdateSlide(int slideId);
    }
}
