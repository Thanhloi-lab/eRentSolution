using eRentSolution.Data.EF;
using eRentSolution.ViewModels.Catalog.ProductImages;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using eRentSolution.Application.Common;
using System.Net.Http.Headers;
using System.IO;
using eRentSolution.Utilities.Exceptions;
using eRentSolution.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.ProductDetails;
using eRentSolution.Utilities.Constants;
using eRentSolution.Data.Enums;

namespace eRentSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly eRentDbContext _context;
        private readonly IStorageService _storageService;
        public ProductService(eRentDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<string>> AddViewcount(int productId)
        {
            var product = await _context.News.FindAsync(productId);
            if(product == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy sản phẩm");
            }
            product.ViewCount += 1;
            try
            {
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<string>("Thêm lượt xem thành công");
            }
            catch(Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình cập nhật");
            }
            
        }
        public async Task<ApiResult<int>> Create(ProductCreateRequest request, Guid userInfoId)
        {
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProduct);
            if (action == null)
                return new ApiErrorResult<int>("Không tìm thấy hành động");
            if(request.Address.ElementAt(0).Equals("_"))
            {
                request.Address.Remove(0);
            }
            var product = new News()
            {
                Name = request.Name,
                DateCreated = DateTime.UtcNow,
                Description = request.Description,
                Address = request.Address,
                ViewCount = 0,
                SeoAlias = request.SeoAlias,
                SeoDescription = request.SeoDescription,
                SeoTitle = request.SeoTitle,
                StatusId = (int)(object)(Status.Private),
                Products = new List<Product>()
                {
                    new Product()
                    {
                        DateCreated = DateTime.UtcNow, 
                        Price = request.Price,
                        OriginalPrice = request.OriginalPrice,
                        Stock = request.Stock,
                        Name = request.SubProductName,
                        Detail = request.Detail,
                        Length = request.Length,
                        Width = request.Width
                    }
                },
                Censors = new List<Censor>()
                {
                    new Censor()
                    {
                        ActionId = action.Id,
                        UserId = userInfoId,
                        Date = DateTime.UtcNow
                    }
                },
            };
            // Luu Anh
            if (request.ThumbnailImage != null)
            {
                product.Products.ElementAt(0).ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Image of " + product.Name,
                        DateCreated = DateTime.UtcNow,
                        FileSize = request.ThumbnailImage.Length,
                        IsDefault = true,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                    }
                };
            }
            else
            {
                return new ApiErrorResult<int>("Ảnh không tồn tại");
            }

            int result = 0;
            try
            {
                await _context.News.AddAsync(product);
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<int>("Lỗi trong quá trình thực hiện thao tác");
            }

            if (result > 0)
            {
                return new ApiSuccessResult<int>(product.Id);
            }
                
            return new ApiErrorResult<int>("Thêm sản phẩm thất bại, vui lòng thử lại sau");
        }
        public async Task<ApiResult<string>> Delete(int productId)
        {
            var product = await _context.News.FindAsync(productId);
            if (product == null)
            {
                throw new eRentException($"Cannot find a product: { product.Id}");
            }
            var details = await GetDetailsByProductId(productId);
            foreach (var item in details.ResultObject)
            {
                var Imgages = _context.ProductImages.Where(p => p.ProductId == item.Id);
                foreach (var image in Imgages)
                {
                    int isDeleteSuccess = _storageService.DeleteFile(image.ImagePath);
                    if (isDeleteSuccess == -1)
                        return new ApiErrorResult<string>("Xóa ảnh thất bại vui lòng thử lại trong giây lát");
                }
            }
            // Lay anh
            

            var censors = await _context.Censors.Where(x => x.NewsId == productId).ToListAsync();
            foreach (var item in censors)
            {
                _context.Censors.Remove(item);
            }

            _context.News.Remove(product);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            
            if (result != 0)
                return new ApiSuccessResult<string>("Xóa ảnh thành công");
            else
                return new ApiErrorResult<string>("Xóa ảnh thất bại, vui lòng thử lại sau");
        }
        public async Task<ApiResult<string>> Hide(int productId, Guid userInfoId)
        {
            var product = await _context.News.FindAsync(productId);
            if (product == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy sản phẩm");
            }

            var action = await _context.UserActions
                .FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.HideProduct);
            if (action == null)
                return new ApiErrorResult<string>("Không tìm thấy hành động");
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserId = userInfoId,
                NewsId = product.Id,
                Date = DateTime.UtcNow
            };
            await _context.Censors.AddAsync(censor);

            product.StatusId = (int)(object)Status.Private;
            _context.News.Update(product);

            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }

            if (result != 0)
                return new ApiSuccessResult<string>("Ẩn sản phẩm thành công");
            else
                return new ApiErrorResult<string>("Ẩn sản phẩm thất bại");
        }
        public async Task<ApiResult<string>> Show(int productId, Guid userInfoId)
        {
            var product = await _context.News.FindAsync(productId);
            if (product == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy sản phẩm");
            }
            product.StatusId = (int)(object)Status.Public;
            _context.News.Update(product);

            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }

            if (result > 0)
            {
                var action = await _context.UserActions
                            .FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.ShowProduct);
                if (action == null)
                    return new ApiErrorResult<string>("Không tìm thấy hành động");
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    UserId = userInfoId,
                    NewsId = product.Id,
                    Date = DateTime.UtcNow
                };
                await _context.Censors.AddAsync(censor);

                try
                {
                    result = await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
                }
            }

            if (result != 0)
                return new ApiSuccessResult<string>("Hiện sản phẩm thành công");
            else
                return new ApiErrorResult<string>("Hiện sản phẩm thất bại");
        }
        public async Task<ApiResult<string>> ActiveProduct(int productId, Guid userInfoId)
        {
            var product = await _context.News.FindAsync(productId);
            if (product == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy sản phẩm");
            }
            if(product.StatusId == (int)(object)Status.InActive)
                product.StatusId = (int)(object)Status.Private;
            else
                product.StatusId = (int)(object)Status.Active;
            _context.News.Update(product);

            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }

            if (result >0)
            {
                var action = await _context.UserActions
                            .FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.ActiveProduct);
                if(action == null)
                    return new ApiErrorResult<string>("Không tìm thấy hành động");
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    UserId = userInfoId,
                    NewsId = product.Id,
                    Date = DateTime.UtcNow
                };
                await _context.Censors.AddAsync(censor);

                try
                {
                    result = await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
                }
            }

            if (result != 0)
                return new ApiSuccessResult<string>("Cho phép sản phẩm hiển thị thành công");
            else
                return new ApiErrorResult<string>("Cho phép sản phẩm hiển thị thất bại");
        }
        public async Task<ApiResult<string>> InActiveProduct(int productId, Guid userInfoId)
        {
            var product = await _context.News.FindAsync(productId);
            if (product == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy sản phẩm");
            }

            var action = await _context.UserActions
                .FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.InActiveProduct);
            if (action == null)
                return new ApiErrorResult<string>("Không tìm thấy hành động");
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserId = userInfoId,
                NewsId = product.Id,
                Date = DateTime.UtcNow
            };
            await _context.Censors.AddAsync(censor);
            _context.News.Update(product);
            product.StatusId = (int)(object)Status.InActive;

            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }

            if (result != 0)
                return new ApiSuccessResult<string>("Khóa sản phẩm thành công");
            else
                return new ApiErrorResult<string>("Khóa sản phẩm thất bại");
        }
        public async Task<ApiResult<string>> UpdatePrice(int productDetailId ,decimal newPrice, Guid userInfoId)
        {
            var productDetail = await _context.Products.FindAsync(productDetailId);
            if (productDetail == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy chi tiết sản phẩm");
            }
            productDetail.Price = newPrice;

            var product = await _context.News.FirstOrDefaultAsync(x => x.Id == productDetail.NewsId);
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdatePriceProduct);
            if (action == null)
                return new ApiErrorResult<string>("Không tìm thấy hành động");
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserId = userInfoId,
                NewsId = product.Id,
                Date = DateTime.UtcNow
            };
            await _context.Censors.AddAsync(censor);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
                return new ApiSuccessResult<string>("Cập nhật giá thành công");
            else
                return new ApiErrorResult<string>("Cập nhật giá thất bại");
        }
        public async Task<ApiResult<string>> UpdateStock(int productDetailId, int addedQuantity, Guid userInfoId)
        {
            var productDetail = await _context.Products.FindAsync(productDetailId);
            if (productDetail == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy chi tiết sản phẩm");
            }
            productDetail.Stock += addedQuantity;

            var product = await _context.News.FirstOrDefaultAsync(x => x.Id == productDetail.NewsId);
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateStockProduct);
            if (action == null)
                return new ApiErrorResult<string>("Không tìm thấy hành động");
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserId = userInfoId,
                NewsId = product.Id,
                Date = DateTime.UtcNow

            };
            await _context.Censors.AddAsync(censor);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
                return new ApiSuccessResult<string>("Cập nhật tồn kho thành công");
            else
                return new ApiErrorResult<string>("Cập nhật tồn kho thất bại");
        }
        public async Task<ApiResult<string>> Update(ProductUpdateRequest request, Guid userInfoId)
        {
            var product = await _context.News.FindAsync(request.Id);
            if (product == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy sản phẩm");
            }
            product.Name = request.Name;
            product.Description = request.Description;
            product.SeoAlias = request.SeoAlias;
            product.SeoTitle = request.SeoTitle;
            product.SeoDescription = request.SeoDescription;
            product.Address = request.Address;
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateProduct);
            if (action == null)
                return new ApiErrorResult<string>("Không tìm thấy hành động");
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserId = userInfoId,
                NewsId = product.Id,
                Date = DateTime.UtcNow
            };
            await _context.Censors.AddAsync(censor);

            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
                return new ApiSuccessResult<string>("Cập nhật sản phẩm thành công");
            else
                return new ApiErrorResult<string>("Cập nhật sản phẩm thất bại");
        }
        public async Task<ApiResult<string>> CreateFeature(int productId, Guid userInfoId)
        {
            var product = await _context.News.FindAsync(productId);
            if (product == null)
            {
                return new ApiErrorResult<string>($"Sản phẩm không tồn tại");
            }
            product.IsFeatured = Status.Active;
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateFeatureProduct);
            if (action == null)
                return new ApiErrorResult<string>("Không tìm thấy hành động");
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserId = userInfoId,
                NewsId = product.Id,
                Date = DateTime.UtcNow
            };
            await _context.Censors.AddAsync(censor);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            };
            if (result > 0)
                return new ApiSuccessResult<string>("Tạo sản phẩm nổi bật thành công");
            else
                return new ApiErrorResult<string>("Tạo sản phẩm nổi bật thất bại");
        }
        public async Task<ApiResult<string>> DeleteFeature(int productId, Guid userInfoId)
        {
            var product = await _context.News.FindAsync(productId);
            if (product == null)
            {
                return new ApiErrorResult<string>($"Sản phẩm không tồn tại");
            }
            product.IsFeatured = Status.InActive;
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.DeleteFeatureProduct);
            if (action == null)
                return new ApiErrorResult<string>("Không tìm thấy hành động");
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserId = userInfoId,
                NewsId = product.Id,
                Date = DateTime.UtcNow
            };
            await _context.Censors.AddAsync(censor);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
                return new ApiSuccessResult<string>("Hủy sản phẩm nổi bật thành công");
            else
                return new ApiErrorResult<string>("Hủy sản phẩm nổi bật thất bại");
        }
        public async Task<ApiResult<string>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var product = await _context.News.FindAsync(id);
            if (product == null)
            {
                return new ApiErrorResult<string>($"Sản phẩm không tồn tại");
            }

            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.NewsInCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id) && x.NewsId == id);

                if (productInCategory != null && category.Selected == false)
                {
                    _context.NewsInCategories.Remove(productInCategory);
                }
                if (productInCategory == null && category.Selected == true)
                {
                    await _context.NewsInCategories.AddAsync(new NewsInCategory()
                    {
                        CategoryId = int.Parse(category.Id),
                        NewsId = id,
                    });
                }
            }
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
                return new ApiSuccessResult<string>("Gán danh mục thành công");
            return new ApiErrorResult<string>("Gán danh mục thất bại");
        }
        public async Task<ApiResult<string>> UpdateDetail(ProductDetailUpdateRequest request, Guid userInfoId)
        {
            var productDetail = await _context.Products.FindAsync(request.Id);
            if (productDetail == null)
            {
                return new ApiErrorResult<string>($"Sản phẩm id không tồn tại");
            }
            productDetail.Name = request.ProductDetailName;
            productDetail.Detail = request.Detail;
            productDetail.Width = request.Width;
            productDetail.Length = request.Length;
            productDetail.Stock = request.Stock;
            productDetail.Price = request.Price;
            productDetail.OriginalPrice = request.OriginalPrice;
            _context.Products.Update(productDetail);
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateProductDetail);
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserId = userInfoId,
                NewsId = productDetail.NewsId,
                Date = DateTime.UtcNow
            };
            
            await _context.Censors.AddAsync(censor);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
                return new ApiSuccessResult<string>("Cập nhật chi tiết thành công");
            else
                return new ApiErrorResult<string>("Cập nhật chi tiết thất bại");
        }
        public async Task<ApiResult<string>> IsMyProduct(Guid userId, int productId)
        {
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProduct);
            var query = from p in _context.News
                        join cen in _context.Censors on p.Id equals cen.NewsId
                        where cen.UserId == userId && cen.ActionId == action.Id && p.Id == productId
                        select new { p };

            if (query.Count() > 0)
                return new ApiSuccessResult<string>("Sản phẩm thuộc về tài khoản hiện tại");
            else
                return new ApiErrorResult<string>("Sản phẩm không thuộc về tài khoản hiện tại");
        }
        public async Task<ApiResult<string>> AddDetail(ProductDetailCreateRequest request, Guid userInfoId)
        {
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProductDetail);
            var product = await _context.News.FindAsync(request.ProductId);
            if (product == null)
                return new ApiErrorResult<string>("Sản phẩm không tồn tại");
            var productDetail = new Product()
            {
                DateCreated = DateTime.UtcNow,
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                Name = request.ProductDetailName,
                Detail = request.Detail,
                Length = request.Length,
                Width = request.Width,
                NewsId = request.ProductId
            };
            // Luu Anh
            if (request.Image != null)
            {
                productDetail.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Image of " + productDetail.Name,
                        DateCreated = DateTime.UtcNow,
                        FileSize = request.Image.Length,
                        IsDefault = true,
                        ImagePath = await this.SaveFile(request.Image),
                    }
                };
            }
            else
            {
                return new ApiErrorResult<string>("Ảnh đính kèm không tồn tại");
            }
            await _context.Products.AddAsync(productDetail);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result < 1)
                return new ApiErrorResult<string>("Thêm ảnh thất bại");
            var censors = new Censor()
            {
                ActionId = action.Id,
                UserId = userInfoId,
                Date = DateTime.UtcNow,
                NewsId = product.Id
            };
            await _context.Censors.AddAsync(censors);
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
                return new ApiSuccessResult<string>("Thêm ảnh thành công");
            return new ApiErrorResult<string>("Thêm ảnh thất bại");
        }
        public async Task<ApiResult<string>> DeleteDetail(int productDetailId, Guid userId)
        {
            var detail = await _context.Products.FindAsync(productDetailId);
            if (detail == null)
                return new ApiErrorResult<string>("Không tìm thấy chi tiết sản phẩm");

            _context.Products.Remove(detail);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
            {
                var Imgages = _context.ProductImages.Where(p => p.ProductId == productDetailId);
                foreach (var image in Imgages)
                {
                    int isDeleteSuccess = _storageService.DeleteFile(image.ImagePath);
                    if (isDeleteSuccess == -1)
                        return new ApiErrorResult<string>("Xóa sản phẩm thất bại, vui lòng thử lại sau");
                }

                var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.DeleteDetail);
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    Date = DateTime.UtcNow,
                    NewsId = detail.NewsId,
                    UserId = userId,
                };
                _context.Censors.Add(censor);
                try
                {
                    result = await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
                }
                if (result > 0)
                    return new ApiSuccessResult<string>("Xóa chi tiết thành công");
            }
            return new ApiErrorResult<string>("Xóa chi tiết thất bại");
        }
        public async Task<ApiResult<ProductDetailViewModel>> GetProductDetailById(int productDetailId)
        {
            var productDetail = await _context.Products.FindAsync(productDetailId);
            if (productDetail == null)
                return new ApiErrorResult<ProductDetailViewModel>("Không tìm thấy chi tiết sản phẩm");

            var images = await GetListImageByProductDetailId(productDetailId);
            if(!images.IsSuccessed)
                return new ApiErrorResult<ProductDetailViewModel>(images.Message);

            var viewModel = new ProductDetailViewModel()
            {
                Id = productDetail.Id,
                DateCreated = productDetail.DateCreated,
                Detail = productDetail.Detail,
                Images = images.ResultObject,
                Length = productDetail.Length,
                Width = productDetail.Width,
                OriginalPrice = productDetail.OriginalPrice,
                ProductDetailName = productDetail.Name,
                Price = productDetail.Price,
                Stock = productDetail.Stock,
                ProductId = productDetail.NewsId
            };
            return new ApiSuccessResult<ProductDetailViewModel>(viewModel);
        }
        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetAllPaging(GetProductPagingRequest request)
        {
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProduct);
            var query = from p in _context.News
                            //join pd in _context.ProductDetails on p.Id equals pd.Id
                        join pic in _context.NewsInCategories on p.Id equals pic.NewsId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join cen in _context.Censors on p.Id equals cen.NewsId
                        join u in _context.AppUsers on cen.UserId equals u.Id
                        where u.Status == Status.Active
                             && cen.ActionId == action.Id
                            //&& pd.IsThumbnail == true
                        select new { p, c, pic };//, pd};

            if (request.Status != null && request.Status.HasValue)
            {
                 query = query.Where(x => x.p.StatusId == request.Status);
            }
            if (request.Keyword != null)
            {
                query = query.Where(x => x.p.Name.Contains(request.Keyword));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            if (request.Address != null)
            {
                string[] address = request.Address.Split("_");
                if (request.Address.Contains("/"))
                {
                    query = query.Where(x => x.p.Address.Contains(address[1]));
                }
                else
                {
                    foreach (var item in address)
                    {
                        query = query.Where(x => x.p.Address.Contains(item));
                    }
                }
            }
            if(request.IsGuess==true)
            {
                query = query.Where(x => x.p.StatusId == (int)(object)(Status.Active));
            }

            int totalRow = await query.CountAsync();
            List<ProductViewModel> data = new List<ProductViewModel>();
            if (request.IsStatisticMonth!=null)
            {
                if(request.IsStatisticMonth==true)
                {
                    data = await query.OrderByDescending(x => x.p.ViewCount)
                        .Skip(request.PageSize * (request.PageIndex - 1))
                        .Take(request.PageSize)
                        .Select(x => new ProductViewModel()
                        {
                            Id = x.p.Id,
                            DateCreated = x.p.DateCreated,
                            Description = x.p.Description,
                            Name = x.p.Name,
                            SeoAlias = x.p.SeoAlias,
                            SeoDescription = x.p.SeoDescription,
                            SeoTitle = x.p.SeoTitle,
                            ViewCount = x.p.ViewCount,
                            StatusId = x.p.StatusId,
                            Address = x.p.Address
                        }).Distinct().ToListAsync();
                    foreach (var item in data)
                    {
                        int months = (DateTime.UtcNow.Year - item.DateCreated.Year) * 12 + DateTime.UtcNow.Month - item.DateCreated.Month;
                        if(months > 0)
                            item.ViewCount = item.ViewCount / months;
                    }
                    
                }
                else
                {
                    data = await query.OrderByDescending(x => x.p.ViewCount)
                       .Skip(request.PageSize * (request.PageIndex - 1))
                       .Take(request.PageSize)
                       .Select(x => new ProductViewModel()
                       {
                           Id = x.p.Id,
                           DateCreated = x.p.DateCreated,
                           Description = x.p.Description,
                           Name = x.p.Name,
                           SeoAlias = x.p.SeoAlias,
                           SeoDescription = x.p.SeoDescription,
                           SeoTitle = x.p.SeoTitle,
                           ViewCount = x.p.ViewCount,
                           StatusId = x.p.StatusId,
                           Address = x.p.Address
                       }).Distinct().ToListAsync();
                }
                data.Sort(new ViewCountComparer());
            }    
            else
            {
                data = await query.OrderBy(x => x.p.Id)
                       .Skip(request.PageSize * (request.PageIndex - 1))
                       .Take(request.PageSize)
                       .Select(x => new ProductViewModel()
                       {
                           Id = x.p.Id,
                           DateCreated = x.p.DateCreated,
                           Description = x.p.Description,
                           Name = x.p.Name,
                           SeoAlias = x.p.SeoAlias,
                           SeoDescription = x.p.SeoDescription,
                           SeoTitle = x.p.SeoTitle,
                           ViewCount = x.p.ViewCount,
                           StatusId = x.p.StatusId,
                           Address = x.p.Address
                       }).Distinct().ToListAsync();
            }
               
            var products = new List<ProductViewModel>();
            foreach (var item in data)
            {
                var productDetails = await GetDetailsByProductId(item.Id);
                item.ProductDetailViewModels = productDetails.ResultObject;
                var status = await _context.NewsStatuses.FirstOrDefaultAsync(x => x.Id == item.StatusId);
                item.Status = status.StatusName;
                foreach (var productDetail in productDetails.ResultObject)
                {
                    item.Stock += productDetail.Stock;
                }
            }
            foreach (var item in data)
            {
                for (int  i = 0;  i <item.ProductDetailViewModels.Count;  i++)
                {
                    if(request.MinPrice!=null && request.MaxPrice!=null)
                    {
                        if (item.ProductDetailViewModels.ElementAt(i).Price >= request.MinPrice && item.ProductDetailViewModels.ElementAt(i).Price <= request.MaxPrice)
                        {
                            products.Add(item);
                            break;
                        }
                    }
                }
            }
            var page = new PagedResult<ProductViewModel>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow
            };
            if (request.MinPrice != null && request.MaxPrice != null)
            {
                page.Items = products;
            }
            else
            {
                page.Items = data;
            }
            return new ApiSuccessResult<PagedResult<ProductViewModel>>(page);
        }
        public async Task<ApiResult<ProductViewModel>> GetById(int id)
        {
            var product = await _context.News.FindAsync(id);
            if (product == null)
                return new ApiErrorResult<ProductViewModel>("Không tìm thấy sản phẩm");

            var categories = await (from c in _context.Categories
                                    join pic in _context.NewsInCategories on c.Id equals pic.CategoryId
                                    where pic.NewsId == id
                                    select c.Name).ToListAsync();

            var productDetails = await GetDetailsByProductId(product.Id);
            if (!productDetails.IsSuccessed)
                return new ApiErrorResult<ProductViewModel>(productDetails.Message);
            var status = await _context.NewsStatuses.FirstOrDefaultAsync(x => x.Id == product.StatusId);
            var productViewModel = new ProductViewModel()
            {
                DateCreated = product.DateCreated,
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                ViewCount = product.ViewCount,
                ProductDetailViewModels = productDetails.ResultObject,
                StatusId = product.StatusId,
                Categories = categories,
                IsFeatured = product.IsFeatured,
                Address = product.Address,
                Status = status.StatusName
            };
            productViewModel.Address = productViewModel.Address.Replace("/", ", ");
            productViewModel.Address = productViewModel.Address.Replace("_", ", ");
            var productImages = await GetListImage(product.Id);
            if (!productImages.IsSuccessed)
                return new ApiErrorResult<ProductViewModel>(productImages.Message);

            foreach (var item in productImages.ResultObject)
            {
                if (item.IsDefault)
                {
                    productViewModel.ThumbnailImage = item.ImagePath;
                    break;
                }
            }
            foreach (var productDetail in productDetails.ResultObject)
            {
                productViewModel.Stock += productDetail.Stock;
            }

            return new ApiSuccessResult<ProductViewModel>(productViewModel);
        }
        public async Task<ApiResult<List<ProductDetailViewModel>>> GetDetailsByProductId(int productId)
        {
            var query = from pd in _context.Products
                        join p in _context.News on pd.NewsId equals p.Id
                        where pd.NewsId == productId
                        select new { pd };
            var productDetails = await query.Select(x => new ProductDetailViewModel()
            {
                Id = x.pd.Id,
                DateCreated = x.pd.DateCreated,
                OriginalPrice = x.pd.OriginalPrice,
                Price = x.pd.Price,
                ProductDetailName = x.pd.Name,
                Stock = x.pd.Stock,
                Width = x.pd.Width,
                Length = x.pd.Length,
                Detail = x.pd.Detail,
            }).ToListAsync();
            foreach (var item in productDetails)
            {
                var result = await GetListImageByProductDetailId(item.Id);
                if (!result.IsSuccessed)
                    return new ApiErrorResult<List<ProductDetailViewModel>>(result.Message);

                item.Images = result.ResultObject;
            }
            return new ApiSuccessResult<List<ProductDetailViewModel>>(productDetails);
        }
        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetFeaturedProducts(GetProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.News
                        join pd in _context.Products on p.Id equals pd.NewsId
                        join pic in _context.NewsInCategories on p.Id equals pic.NewsId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        where p.IsFeatured == Status.Active &&  p.StatusId == (int)(object)Status.Active
                        select new { p, pd, pic };
            if (request.Keyword != null)
            {
                query = query.Where(x => x.p.Name.Contains(request.Keyword));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            if (request.IsGuess == true)
            {
                query = query.Where(x => x.p.StatusId == (int)(object)(Status.Active));
            }
            var totalRow = query.Count();
            var data = await query.OrderByDescending(x => x.p.DateCreated)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    Name = x.p.Name,
                    SeoAlias = x.p.SeoAlias,
                    SeoDescription = x.p.SeoDescription,
                    SeoTitle = x.p.SeoTitle,
                    ViewCount = x.p.ViewCount,
                    StatusId = x.p.StatusId,
                    Address = x.p.Address
                }).Distinct().ToListAsync();

            foreach (var item in data)
            {
                var productDetails = await GetDetailsByProductId(item.Id);
                item.ProductDetailViewModels = productDetails.ResultObject;
                var status = await _context.NewsStatuses.FirstOrDefaultAsync(x => x.Id == item.StatusId);
                item.Status = status.StatusName;
                foreach (var productDetail in productDetails.ResultObject)
                {
                    item.Stock += productDetail.Stock;
                }
            }
            var products = new List<ProductViewModel>();
            foreach (var item in data)
            {
                for (int i = 0; i < item.ProductDetailViewModels.Count; i++)
                {
                    if (request.MinPrice != null && request.MaxPrice != null)
                    {
                        if (item.ProductDetailViewModels.ElementAt(i).Price > request.MinPrice && item.ProductDetailViewModels.ElementAt(i).Price < request.MaxPrice)
                        {
                            products.Add(item);
                            break;
                        }
                    }
                }
            }
            var pageResult = new PagedResult<ProductViewModel>()
            {
                Items = products,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };
            if (request.MinPrice != null && request.MaxPrice != null)
            {
                pageResult.Items = products;
            }
            else
            {
                pageResult.Items = data;
            }
            return new ApiSuccessResult<PagedResult<ProductViewModel>>(pageResult);
        }
        public async Task<ApiResult<List<ProductViewModel>>> GetLastestProducts(int take)
        {
            //1. Select join
            var query = from p in _context.News
                        join pd in _context.Products on p.Id equals pd.NewsId
                        join pic in _context.NewsInCategories on p.Id equals pic.NewsId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        select new { p, pd };//, pic};

            var totalRow = query.Count();
            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take).OrderBy(x => x.p.DateCreated)
                .Select(x => new ProductViewModel()
                {

                    Id = x.p.Id,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    //Details = x.p.Details,
                    Name = x.p.Name,
                    SeoAlias = x.p.SeoAlias,
                    SeoDescription = x.p.SeoDescription,
                    SeoTitle = x.p.SeoTitle,
                    ViewCount = x.p.ViewCount,
                    StatusId = x.p.StatusId,
                    Address = x.p.Address
                }).Distinct().ToListAsync();
            foreach (var item in data)
            {
                var productDetails = await GetDetailsByProductId(item.Id);
                item.ProductDetailViewModels = productDetails.ResultObject;
                foreach (var productDetail in productDetails.ResultObject)
                {
                    item.Stock += productDetail.Stock;
                }
            }
            return new ApiSuccessResult<List<ProductViewModel>>(data);
        }
        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetPageProductByUserID(GetProductPagingRequest request, Guid userId)
        {
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProduct);
            var query = from p in _context.News
                        join cen in _context.Censors on p.Id equals cen.NewsId
                        join pic in _context.NewsInCategories on p.Id equals pic.NewsId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where cen.UserId == userId 
                             && cen.ActionId == action.Id
                        select new { p, c, pic };

            if (request.Keyword != null)
            {
                query = query.Where(x => x.p.Name.Contains(request.Keyword));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            if (request.Address != null)
            {
                string[] address = request.Address.Split("_");
                if (request.Address.Contains("/"))
                {
                    query = query.Where(x => x.p.Address.Contains(address[1]));
                }
                else
                {
                    foreach (var item in address)
                    {
                        query = query.Where(x => x.p.Address.Contains(item));
                    }
                }
            }
            if (request.IsGuess == true)
            {
                query = query.Where(x => x.p.StatusId == (int)(object)(Status.Active));
            }
            
            int totalRow = await query.CountAsync();
            var data = await query.OrderByDescending(x => x.p.DateCreated)
                .Skip(request.PageSize * (request.PageIndex - 1))
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                DateCreated = x.p.DateCreated,
                Description = x.p.Description,
                Name = x.p.Name,
                SeoAlias = x.p.SeoAlias,
                SeoDescription = x.p.SeoDescription,
                SeoTitle = x.p.SeoTitle,
                ViewCount = x.p.ViewCount,
                StatusId = x.p.StatusId,
                Address = x.p.Address.Replace("_", ", ").Replace("/", ", "),
            }).Distinct().ToListAsync();
            
            foreach (var item in data)
            {
                var productDetails = await GetDetailsByProductId(item.Id);
                item.ProductDetailViewModels = productDetails.ResultObject;
                var status = await _context.NewsStatuses.FirstOrDefaultAsync(x => x.Id == item.StatusId);
                item.Status = status.StatusName; 
                foreach (var productDetail in productDetails.ResultObject)
                {
                    item.Stock += productDetail.Stock;
                }
            }
            var products = new List<ProductViewModel>();
            foreach (var item in data)
            {
                for (int i = 0; i < item.ProductDetailViewModels.Count; i++)
                {
                    if (request.MinPrice != null && request.MaxPrice != null)
                    {
                        if (item.ProductDetailViewModels.ElementAt(i).Price >= request.MinPrice && item.ProductDetailViewModels.ElementAt(i).Price <= request.MaxPrice)
                        {
                            products.Add(item);
                            break;
                        }
                    }
                }
            }
            var page = new PagedResult<ProductViewModel>()
            {
                
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow
            };
            if (request.MinPrice != null && request.MaxPrice != null)
            {
                page.Items = products;
            }    
            else
            {
                page.Items = data;
            }    
            return new ApiSuccessResult<PagedResult<ProductViewModel>>(page);
        }
        public async Task<ApiResult<PagedResult<UserProductStatisticViewModel>>> GetStatisticUserProduct(GetProductPagingRequest request)
        {
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProduct);
            var query = from p in _context.News
                        join cen in _context.Censors on p.Id equals cen.NewsId
                        //join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        //from pic in ppic.DefaultIfEmpty()
                        join u in _context.AppUsers on cen.UserId equals u.Id
                        where cen.ActionId == action.Id
                        select new {u, p};
            if(request.CategoryId!=null)
            {
                query = (from qr in query
                        join pic in _context.NewsInCategories on qr.p.Id equals pic.NewsId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        where pic.CategoryId == request.CategoryId
                        select new { qr.u, qr.p }).Distinct();
            }
            if (request.Keyword != null)
            {
                query = query.Where(x => x.u.UserName.Contains(request.Keyword)
                           || x.u.PhoneNumber.Contains(request.Keyword));
            }

            var data = await query.GroupBy(x=> new {x.u.Id, x.u.UserName})
                .Select(x => new UserProductStatisticViewModel()
                {
                    UserId = x.Key.Id,
                    UserName = x.Key.UserName,
                    ViewCount = x.Sum(x=>x.p.ViewCount),
                    AmountProducts = x.Count()
                }).ToListAsync();
            
            var totalRow = data.Count();
            data.Skip(request.PageSize * (request.PageIndex - 1))
                .Take(request.PageSize);
            var page = new PagedResult<UserProductStatisticViewModel>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow
            };
            return new ApiSuccessResult<PagedResult<UserProductStatisticViewModel>>(page);
        }
        //----------------Images-------
        // No Done
        public async Task<ApiResult<string>> DeleteImage(int imageId, Guid userId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                return new ApiErrorResult<string>("Không tìm thấy ảnh");

            var productDetail = await _context.Products.FindAsync(productImage.ProductId);
            if (productDetail == null)
                return new ApiErrorResult<string>("Không tìm thấy chi tiết sản phẩm");

            var listProductImages = from pd in _context.Products
                                    join i in _context.ProductImages on pd.Id equals i.ProductId
                                    select new { i };
            if (listProductImages.Count() <= 1)
                return new ApiErrorResult<string>("Không thể xóa ảnh cuối cùng của chi tiết này");

            int isDeleteSuccess = _storageService.DeleteFile(productImage.ImagePath);
            if (isDeleteSuccess == -1)
                return new ApiErrorResult<string>("Xóa ảnh không thành công, vui lòng thử lại sau");

            _context.ProductImages.Remove(productImage);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
            {
                var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.DeleteImage);
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    Date = DateTime.UtcNow,
                    NewsId = productDetail.NewsId,
                    UserId = userId,
                };
                _context.Censors.Add(censor);
                return new ApiSuccessResult<string>("Xóa hình ảnh thành công");
            }

            return new ApiErrorResult<string>("Xóa hình ảnh không thành công");
        }
        public async Task<ApiResult<string>> AddImage(ProductImageCreateRequest request, Guid userId)
        {
            var product = await _context.News.FindAsync(request.ProductId);
            if (product == null)
                return new ApiErrorResult<string>($"Sản phẩm id={request.ProductDetailId} không tồn tại");
            var productImage = new ProductImage()
            {
                Caption = request.Caption == null ? "Non-caption" : request.Caption,
                DateCreated = DateTime.UtcNow,
                ProductId = request.ProductDetailId
            };
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
            {
                var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateImage);
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    Date = DateTime.UtcNow,
                    NewsId = product.Id,
                    UserId = userId,
                };
                _context.Censors.Add(censor);
                return new ApiSuccessResult<string>($"Thêm ảnh thành công");
            }
            return new ApiErrorResult<string>("Thêm ảnh thất bại");
        }
        public async Task<ApiResult<string>> UpdateImage(ProductImageUpdateRequest request, Guid userId)
        {
            var productImage = await _context.ProductImages.FindAsync(request.ImageId);
            if (productImage == null)
                return new ApiErrorResult<string>($"Hình ảnh id:{request.ImageId} không tồn tại");
            if (request.ImageFile != null)
            {
                int isDeleteSuccess = _storageService.DeleteFile(productImage.ImagePath);
                if (isDeleteSuccess == -1)
                    return new ApiErrorResult<string>($"Sửa ảnh thất bại, vui lòng thử lại sau vài giây.");
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiErrorResult<string>("Lỗi trong quá trình thực hiện thao tác");
            }
            if (result > 0)
            {
                var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateImage);
                var detail = await _context.Products.FindAsync(productImage.ProductId);
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    Date = DateTime.UtcNow,
                    NewsId = detail.NewsId,
                    UserId = userId,
                };
                _context.Censors.Add(censor);
                return new ApiSuccessResult<string>($"Thêm ảnh thành công");
            }
            return new ApiErrorResult<string>("Thêm ảnh thất bại");
        }
        public async Task<ApiResult<ProductImageViewModel>> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                return new ApiErrorResult<ProductImageViewModel>("Hình ảnh không tồn tại");

            var productViewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                ImagePath = image.ImagePath,
                FileSize = image.FileSize,
                DateCreated = image.DateCreated,
                ProductDetailId = image.ProductId,
                Id = image.Id,
                IsDefault = image.IsDefault
            };
            return new ApiSuccessResult<ProductImageViewModel>(productViewModel);
        }
        public async Task<ApiResult<List<ProductImageViewModel>>> GetListImage(int productId)
        {
            var product = await _context.News.FindAsync(productId);
            if (product == null)
                return new ApiErrorResult<List<ProductImageViewModel>> ("Không tìm thấy sản phẩm");
            var productDetail = await _context.Products.FirstOrDefaultAsync(x => x.NewsId == productId);
            if (productDetail == null)
                return new ApiErrorResult<List<ProductImageViewModel>>("Không tìm thấy chi tiết sản phẩm");

            var images = await _context.ProductImages.Where(x => x.ProductId == productDetail.Id)
                        .Select(i => new ProductImageViewModel()
                        {
                            Caption = i.Caption,
                            ImagePath = i.ImagePath,
                            FileSize = i.FileSize,
                            DateCreated = i.DateCreated,
                            IsDefault = i.IsDefault,
                            ProductDetailId = productDetail.Id,
                            Id = i.Id,
                        }).ToListAsync();
            return new ApiSuccessResult<List<ProductImageViewModel>>(images);
        }
        public async Task<ApiResult<List<ProductImageViewModel>>> GetListImageByProductDetailId(int productDetailId)
        {
            var productDetail = await _context.Products.FindAsync(productDetailId);
            if (productDetail == null)
                return new ApiErrorResult<List<ProductImageViewModel>>("Không tìm thấy chi tiết sản phẩm");
            var images = await _context.ProductImages.Where(x => x.ProductId == productDetail.Id)
                        .Select(i => new ProductImageViewModel()
                        {
                            Caption = i.Caption,
                            ImagePath = i.ImagePath,
                            FileSize = i.FileSize,
                            DateCreated = i.DateCreated,
                            IsDefault = i.IsDefault,
                            ProductDetailId = productDetail.Id,
                            Id = i.Id,
                        }).ToListAsync();
            return new ApiSuccessResult<List<ProductImageViewModel>>(images);
        }
        

        //--------FILE--------
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public class ViewCountComparer : Comparer<ProductViewModel>
        {
            // Compares by Length, Height, and Width.
            public override int Compare(ProductViewModel x, ProductViewModel y)
            {
                if (y.ViewCount.CompareTo(x.ViewCount) != 0)
                {
                    return y.ViewCount.CompareTo(x.ViewCount);
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}
