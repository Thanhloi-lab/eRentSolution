using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eRentSolution.ViewModels.Catalog.ProductImages
{
    public class ProductImageCreateRequest
    {
        [Display(Name = "Chú thích")]
        public string Caption { get; set; }
        [Display(Name = "Mã chi tiết sản phẩm")]
        public int ProductDetailId { get; set; }
        [Display(Name = "Ảnh")]
        public IFormFile ImageFile { get; set; }
        [Display(Name = "Mã  sản phẩm")]
        public int ProductId { get; set; }
    }
}
