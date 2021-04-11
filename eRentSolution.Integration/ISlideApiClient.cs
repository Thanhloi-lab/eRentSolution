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
    }
}
