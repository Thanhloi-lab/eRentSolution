using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eRentSolution.ViewModels.Catalog.ProductImages
{
    public class ProductImageUpdateRequest
    {
        [Display(Name = "Mã hình ảnh")]
        public int ImageId { get; set; }
        [Display(Name = "Ảnh mới")]
        public IFormFile ImageFile { get; set; }
        [Display(Name = "Ảnh cũ")]
        public string OldImageUrl { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }
    }
}
