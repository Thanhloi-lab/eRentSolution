using Microsoft.AspNetCore.Http;

namespace eRentSolution.ViewModels.Catalog.ProductImages
{
    public class ProductImageUpdateRequest
    {
        public int ImageId { get; set; }
        public IFormFile ImageFile { get; set; }
        public string OldImageUrl { get; set; }
    }
}
