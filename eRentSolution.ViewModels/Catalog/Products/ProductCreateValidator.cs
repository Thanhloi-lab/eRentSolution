using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateValidator()
        {
            
            RuleFor(x => x.Detail).NotEmpty().WithMessage("Không thể bỏ trống chi tiết.");
            RuleFor(x => x.Length).NotNull().WithMessage("Không thể bỏ trống chiều dài.")
                .GreaterThan(0).WithMessage("Chiều dài không thể bé hơn 0.");
            RuleFor(x => x.Width).NotNull().WithMessage("Không thể bỏ trống chiều rộng.")
                .GreaterThan(0).WithMessage("Chiều dài không thể bé hơn 0.");
            RuleFor(x => x.Price).NotNull().WithMessage("Không thể bỏ trống giá.")
                .GreaterThan(0).WithMessage("Giá phải lớn hơn 0.");
            RuleFor(x => x.OriginalPrice).NotNull().WithMessage("Không thể bỏ trống giá gốc.")
                .GreaterThan(0).WithMessage("Giá gốc phải lớn hơn 0.");
            RuleFor(x => x.Stock).NotNull().WithMessage("Không thể bỏ trống tồn kho.")
                .GreaterThan(0).WithMessage("Tồn kho phải lớn hơn 0.");
            RuleFor(x => x.SubProductName).NotEmpty().WithMessage("Không thể để trống tên chi tiết sản phẩm.")
                .MaximumLength(200).WithMessage("Tên chi tiết sản phẩm không vượt quá 200 kí tự.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Không thể để trống tên sản phẩm.")
               .MaximumLength(200).WithMessage("Tên sản phẩm không vượt quá 200 kí tự.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Địa chỉ không được để trống.")
               .MaximumLength(300).WithMessage("Địa chỉ không vượt quá 200 kí tự.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.")
               .MaximumLength(200).WithMessage("Mô tả không vượt quá 200 kí tự.");
            RuleFor(x => x.ThumbnailImage).NotNull().WithMessage("Vui lòng chọn ảnh.");
        }
    }
}
