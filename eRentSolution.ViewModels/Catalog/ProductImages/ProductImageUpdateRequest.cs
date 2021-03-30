using Microsoft.AspNetCore.Http;

namespace eRentSolution.ViewModels.Catalog.ProductImages
{
    public class ProductImageUpdateRequest
    {
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public int SortOrder { get; set; }
        public int ImageId { get; set; }
        public IFormFile imageFile { get; set; }
    }
}
