using Microsoft.AspNetCore.Http;

namespace eRentSolution.ViewModels.Catalog.ProductImages
{
    public class ProductImageCreateRequest
    {
        public string Caption { get; set; }
        public int ProductDetailId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
