using eRentSolution.Data.EF;
using eRentSolution.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eRentSolution.Application.Utilities.Slides
{
    public class SlideSerivce : ISlideService
    {
        private readonly eRentDbContext _context;

        public SlideSerivce(eRentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddSlide(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSlide(int slideId)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateSlide(int slideId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SlideViewModel>> GetAll()
        {
            var slides = await _context.Slides.Select(x => new SlideViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                FilePath = x.ImagePath,
                Name = x.Name,
                ProductId = x.ProductId,
                Url = x.Url
            }).ToListAsync();
            return slides;
        }

        
    }
}
