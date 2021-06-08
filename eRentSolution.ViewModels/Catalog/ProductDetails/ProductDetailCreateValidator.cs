using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.ProductDetails
{
    public class ProductDetailCreateValidator : AbstractValidator<ProductDetailCreateRequest>
    {
        public ProductDetailCreateValidator()
        {
            RuleFor(x => x.Image).NotNull().WithMessage("Vui lòng chọn ảnh.");
            RuleFor(x=>x.Detail).NotEmpty().WithMessage("Không thể bỏ trống chi tiết.")
                .MaximumLength(1000).WithMessage("Chi tiết không được vượt quá 1000 kí tự.");
            RuleFor(x => x.Length).NotNull().WithMessage("Không thể bỏ trống chiều dài.")
                .GreaterThan(0).WithMessage("Chiều dài không thể bé hơn 0.");
            RuleFor(x => x.Width).NotNull().WithMessage("Không thể bỏ trống chiều rộng.")
                .GreaterThan(0).WithMessage("Chiều dài không thể bé hơn 0.");
            RuleFor(x => x.ProductDetailName).NotEmpty().WithMessage("Không thể bỏ trống tên chi tiết sản phẩm.")
                .MaximumLength(200).WithMessage("Tên chi tiết sản phẩm không vượt quá 200 kí tự.");
            RuleFor(x => x.Price).NotNull().WithMessage("Không thể bỏ trống giá.")
                .GreaterThan(0).WithMessage("Giá phải lớn hơn 0.");
            RuleFor(x => x.OriginalPrice).NotNull().WithMessage("Không thể bỏ trống giá gốc.")
                .GreaterThan(0).WithMessage("Giá gốc phải lớn hơn 0.");
            RuleFor(x => x.Stock).NotNull().WithMessage("Không thể bỏ trống tồn kho.")
                .GreaterThan(0).WithMessage("Tồn kho phải lớn hơn 0.");
        }
    }
}
