using eRentSolution.Data.EF;
using eRentSolution.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eRentSolution.Data.Entities;
using eRentSolution.Application.Common;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eRentSolution.ViewModels.Common;

namespace eRentSolution.Application.Utilities.Slides
{
    public class SlideService : ISlideService
    {
        private readonly eRentDbContext _context;
        private readonly IStorageService _storageService;
        private string productUrlPattern = "https://localhost:5003/products/";

        public SlideService(eRentDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddSlide(SlideCreateRequest request, Guid userInfoId)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if(product==null)
                return -1;
            var slide = new Slide()
            {
                Name = request.Name,
                Description = request.Description,
                ImagePath = await this.SaveFile(request.ImageFile),
                ProductId = request.ProductId,
                Url = productUrlPattern + request.ProductId,
            };
            await _context.Slides.AddAsync(slide);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteSlide(SlideDeleteRequest request, Guid userInfoId)
        {
            var slide = await _context.Slides.FindAsync(request.Id);
            if (slide == null)
                return false;

            await _storageService.DeleteFileAsync(slide.ImagePath);
            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();
            return true;
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
            foreach (var item in slides)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                item.ProductName = product.Name;
            }
            return slides;
        }

        public async Task<PagedResult<SlideViewModel>> GetAllPaging(GetSlidePagingRequest request)
        {
            var query = from s in _context.Slides
                        join p in _context.Products on s.ProductId equals p.Id
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        select new { s, p, pic };
            if (request.CategoryId != null && request.CategoryId != 0)
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);

            int totalRow = await query.CountAsync();
            var data = await query.Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).Select(x => new SlideViewModel()
            {
                Id = x.s.Id,
                Name = x.s.Name,
                Description = x.s.Description,
                FilePath = x.s.ImagePath,
                ProductId = x.p.Id,
                Url = productUrlPattern + x.s.Id,
                ProductName = x.p.Name
            }).ToListAsync();

            List<SlideViewModel> slides = new List<SlideViewModel>();
            if (totalRow > 1)
            {
                for (int i = 0; i < data.Count - 1; i++)
                {
                    if (data.ElementAt(i).Id == data.ElementAt(i + 1).Id)
                    {
                        totalRow--;
                    }
                    else
                    {
                        slides.Add(data.ElementAt(i));
                    }
                    if (i == data.Count - 2)
                    {
                        slides.Add(data.ElementAt(i + 1));
                    }
                }
            }
            else if (totalRow == 1)
            {
                slides.Add(data.ElementAt(0));
            }

            var page = new PagedResult<SlideViewModel>()
            {
                Items = slides,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = slides.Count
            };
            return page;
        }

        public async Task<SlideViewModel> GetById(int slideId)
        {
            var slide = await _context.Slides.FindAsync(slideId);
            if (slide == null)
                return null;
            var slideViewModel =  new SlideViewModel()
            {
                Id = slide.Id,
                Description = slide.Description,
                FilePath = slide.ImagePath,
                Url = slide.Url,
                Name = slide.Name,
                ProductId = slide.ProductId,
            };
            var product = await _context.Products.FindAsync(slideViewModel.ProductId);
            slideViewModel.ProductName = product.Name;
            return slideViewModel;
        }

        public async Task<bool> UpdateSlide(SlideUpdateRequest request, Guid userInfoId)
        {
            var slide = await _context.Slides.FindAsync(request.Id);
            if (slide == null)
                return false;
            slide.Description = request.Description;
            //slide.ImagePath = await _storageService.SaveFile(request.ImageFile);
            slide.Name = request.Name;
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
