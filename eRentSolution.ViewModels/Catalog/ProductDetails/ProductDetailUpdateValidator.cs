using FluentValidation;

namespace eRentSolution.ViewModels.Catalog.ProductDetails
{
    public class ProductDetailUpdateValidator : AbstractValidator<ProductDetailUpdateRequest>
    {
        public ProductDetailUpdateValidator()
        {
            RuleFor(x => x.Detail).NotEmpty().WithMessage("Không thể bỏ trống chi tiết.");
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
