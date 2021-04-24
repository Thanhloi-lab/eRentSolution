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
       
        //Fixed
        //kiem tra null trc roi moi + vao
        public async Task<bool> AddViewcount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if(product == null)
            {
                throw new eRentException($"Cannot find a product: { product.Id}");
            }
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<int> Create(ProductCreateRequest request, Guid userInfoId)
        {
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProduct);
            var product = new Product()
            {
                Name = request.Name,
                DateCreated = DateTime.UtcNow,
                Description = request.Description,
                //Details = request.Details,
                Address = request.Address,
                ViewCount = 0,
                SeoAlias = request.SeoAlias,
                SeoDescription = request.SeoDescription,
                SeoTitle = request.SeoTitle,
                ProductDetails = new List<ProductDetail>()
                {
                    new ProductDetail()
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
                        UserInfoId = userInfoId,
                        Date = DateTime.UtcNow
                    }
                }
            };
            // Luu Anh
            if (request.ThumbnailImage != null)
            {
                product.ProductDetails.ElementAt(0).ProductImages = new List<ProductImage>()
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
                return -1;
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }
        public async Task<bool> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new eRentException($"Cannot find a product: { product.Id}");
            }
            var details = await GetDetailsByProductId(productId);
            foreach (var item in details)
            {
                var Imgages = _context.ProductImages.Where(p => p.ProductDetailId == item.Id);
                foreach (var image in Imgages)
                {
                    int isDeleteSuccess = _storageService.DeleteFile(image.ImagePath);
                    if (isDeleteSuccess == -1)
                        return false;
                }
            }
            // Lay anh
            

            var censors = await _context.Censors.Where(x => x.ProductId == productId).ToListAsync();
            foreach (var item in censors)
            {
                _context.Censors.Remove(item);
            }

            _context.Products.Remove(product);

            var result = await _context.SaveChangesAsync();
            if (result != 0)
                return true;
            else
                return false;
        }
        public async Task<bool> Hide(int productId, Guid userInfoId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new eRentException($"Cannot find a product: { product.Id}");
            }

            var action = await _context.UserActions
                .FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.HideProduct);
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserInfoId = userInfoId,
                ProductId = product.Id,
                Date = DateTime.UtcNow
            };
            await _context.Censors.AddAsync(censor);

            product.Status = Data.Enums.Status.InActive;

            var result = await _context.SaveChangesAsync();
            if (result != 0)
                return true;
            else
                return false;
        }
        public async Task<bool> Show(int productId, Guid userInfoId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new eRentException($"Cannot find a product: { product.Id}");
            }
            product.Status = Data.Enums.Status.Active;

            var result = await _context.SaveChangesAsync();
            if(result >0)
            {
                var action = await _context.UserActions
                .FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.ShowProduct);
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    UserInfoId = userInfoId,
                    ProductId = product.Id,
                    Date = DateTime.UtcNow
                };
                await _context.Censors.AddAsync(censor);
                result = await _context.SaveChangesAsync();
            }

            if (result != 0)
                return true;
            else
                return false;
        }
        public async Task<bool> UpdatePrice(int productDetailId ,decimal newPrice, Guid userInfoId)
        {
            //var product = await _context.Products.FindAsync(productId);
            //if (product == null)
            //{
            //    return false;
            //}
            var productDetail = await _context.ProductDetails.FindAsync(productDetailId);
            if (productDetail == null)
            {
                return false;
            }
            productDetail.Price = newPrice;

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productDetail.ProductId);
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdatePriceProduct);
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserInfoId = userInfoId,
                ProductId = product.Id,
                Date = DateTime.UtcNow
            };
            await _context.Censors.AddAsync(censor);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateStock(int productDetailId, int addedQuantity, Guid userInfoId)
        {
            var productDetail = await _context.ProductDetails.FindAsync(productDetailId);
            if (productDetail == null)
            {
                return false;
            }
            productDetail.Stock += addedQuantity;

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productDetail.ProductId);
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateStockProduct);
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserInfoId = userInfoId,
                ProductId = product.Id,
                Date = DateTime.UtcNow

            };
            await _context.Censors.AddAsync(censor);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(ProductUpdateRequest request, Guid userInfoId)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                return false;
            }
            product.Name = request.Name;
            product.Description = request.Description;
            //product.Details = request.Details;
            product.SeoAlias = request.SeoAlias;
            product.SeoTitle = request.SeoTitle;
            product.SeoDescription = request.SeoDescription;
            product.Address = request.Address;
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateProduct);
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserInfoId = userInfoId,
                ProductId = product.Id,
                Date = DateTime.UtcNow
            };
            await _context.Censors.AddAsync(censor);

            var result = await _context.SaveChangesAsync();
            return true;
        }
        public async Task<ApiResult<bool>> CreateFeature(int productId, Guid userInfoId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return new ApiErrorResult<bool>($"Sản phẩm id:{productId} không tồn tại");
            }
            product.IsFeatured = Status.Active;
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateFeatureProduct);
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserInfoId = userInfoId,
                ProductId = product.Id,
                Date = DateTime.UtcNow
            };
            await _context.Censors.AddAsync(censor);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }
        public async Task<ApiResult<bool>> DeleteFeature(int productId, Guid userInfoId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return new ApiErrorResult<bool>($"Sản phẩm id:{productId} không tồn tại");
            }
            product.IsFeatured = Status.InActive;
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.DeleteFeatureProduct);
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserInfoId = userInfoId,
                ProductId = product.Id,
                Date = DateTime.UtcNow
            };
            await _context.Censors.AddAsync(censor);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }
        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            var query = from p in _context.Products
                            //join pd in _context.ProductDetails on p.Id equals pd.Id
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                            //&& pd.IsThumbnail == true
                        select new { p, c, pic};//, pd};

            if (request.Keyword != null)
            {
                query = query.Where(x => x.p.Name.Contains(request.Keyword));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).Select(x => new ProductViewModel()
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
                Status = x.p.Status,
                Address = x.p.Address
            }).ToListAsync();

            List<ProductViewModel> products = new List<ProductViewModel>();
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
                        products.Add(data.ElementAt(i));
                    }
                    if (i == data.Count - 2)
                    {
                        products.Add(data.ElementAt(i + 1));
                    }
                }
            }
            else if (totalRow == 1)
            {
                products.Add(data.ElementAt(0));
            }
            foreach (var item in products)
            {
                var productDetails = await GetDetailsByProductId(item.Id);
                item.ProductDetailViewModels = productDetails;
                foreach (var productDetail in productDetails)
                {
                    item.Stock += productDetail.Stock;
                }
            }
            
            var page = new PagedResult<ProductViewModel>()
            {
                Items = products,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow
            };
            return page;
        }
        public async Task<ProductViewModel> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new eRentException($"Cannot find a product: {id}");

            var categories = await (from c in _context.Categories
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                                    where pic.ProductId == id
                                    select c.Name).ToListAsync();

            var productDetails = await GetDetailsByProductId(product.Id);

            var productViewModel = new ProductViewModel()
            {
                DateCreated = product.DateCreated,
                Description = product.Description,
                //Details = product.Details,
                Id = product.Id,
                Name = product.Name,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                ViewCount = product.ViewCount,
                ProductDetailViewModels = productDetails,
                Status = product.Status,
                Categories = categories,
                IsFeatured = product.IsFeatured,
                Address = product.Address
            };
            var productImages = await GetListImage(product.Id);
            foreach (var item in productImages)
            {
                if (item.IsDefault)
                {
                    productViewModel.ThumbnailImage = item.ImagePath;
                    break;
                }
            }
            foreach (var productDetail in productDetails)
            {
                productViewModel.Stock += productDetail.Stock;
            }

            return productViewModel;
        }
        public async Task<List<ProductDetailViewModel>> GetDetailsByProductId(int productId)
        {
            var query =from pd in _context.ProductDetails
                        join p in _context.Products on pd.ProductId equals p.Id
                       where pd.ProductId == productId
                        select new { pd };
            var productDetails = await query.Select(x => new ProductDetailViewModel()
            {
                Id = x.pd.Id,
                DateCreated = x.pd.DateCreated,
                OriginalPrice = x.pd.OriginalPrice,
                Price = x.pd.Price,
                ProductDetailName = x.pd.Name,
                Stock = x.pd.Stock, 
                Width=x.pd.Width,
                Length = x.pd.Length,
                Detail = x.pd.Detail,
            }).ToListAsync();
            foreach (var item in productDetails)
            {
                item.Images = await GetListImageByProductDetailId(item.Id);
            }
            return productDetails;
        }
        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new ApiErrorResult<bool>($"Sản phẩm id:{id} không tồn tại");
            }

            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductInCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id) && x.ProductId == id);

                if (productInCategory != null && category.Selected == false)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                if (productInCategory == null && category.Selected == true)
                {
                    await _context.ProductInCategories.AddAsync(new ProductInCategory()
                    {
                        CategoryId = int.Parse(category.Id),
                        ProductId = id,
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<PagedResult<ProductViewModel>> GetFeaturedProducts(GetProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pd in _context.ProductDetails on p.Id equals pd.ProductId

                        where p.IsFeatured == Status.Active
                        select new { p, pd };

            var totalRow = query.Count();
            var data = await query.OrderByDescending(x => x.p.DateCreated)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                   // Details = x.p.Details,
                    Name = x.p.Name,
                    SeoAlias = x.p.SeoAlias,
                    SeoDescription = x.p.SeoDescription,
                    SeoTitle = x.p.SeoTitle,
                    ViewCount = x.p.ViewCount,
                    Status = x.p.Status,
                    Address = x.p.Address
                }).ToListAsync();

            List<ProductViewModel> products = new List<ProductViewModel>();
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
                        products.Add(data.ElementAt(i));
                    }
                    if (i == data.Count - 2)
                    {
                        products.Add(data.ElementAt(i + 1));
                    }
                }
            }
            else if (totalRow == 1)
            {
                products.Add(data.ElementAt(0));
            }
            foreach (var item in products)
            {
                var productDetails = await GetDetailsByProductId(item.Id);
                item.ProductDetailViewModels = productDetails;
                foreach (var productDetail in productDetails)
                {
                    item.Stock += productDetail.Stock;
                }
            }
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                Items = products,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };
            return pageResult;
        }
        public async Task<List<ProductViewModel>> GetLastestProducts(int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pd in _context.ProductDetails on p.Id equals pd.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        select new { p, pd };//, pic};

            var totalRow = query.Count();
            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
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
                    Status = x.p.Status, 
                    Address = x.p.Address
                }).Distinct().ToListAsync();

            List<ProductViewModel> products = new List<ProductViewModel>();
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
                        products.Add(data.ElementAt(i));
                    }
                    if (i == data.Count - 2)
                    {
                        products.Add(data.ElementAt(i + 1));
                    }
                }
            }
            else if (totalRow == 1)
            {
                products.Add(data.ElementAt(0));
            }

            foreach (var item in products)
            {
                var productDetails = await GetDetailsByProductId(item.Id);
                item.ProductDetailViewModels = productDetails;
                foreach (var productDetail in productDetails)
                {
                    item.Stock += productDetail.Stock;
                }
            }
            return products;
        }
        public async Task<PagedResult<ProductViewModel>> GetPageProductByUserID(GetProductPagingRequest request, Guid userId)
        {
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProduct);
            var query = from p in _context.Products
                        join cen in _context.Censors on p.Id equals cen.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where cen.UserInfoId == userId && p.Status == Status.Active
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
            int totalRow = await query.CountAsync();
            var data = await query.Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).Select(x => new ProductViewModel()
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
                Status = x.p.Status,
                Address = x.p.Address
            }).ToListAsync();

            List<ProductViewModel> products = new List<ProductViewModel>();
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
                        products.Add(data.ElementAt(i));
                    }
                    if (i == data.Count - 2)
                    {
                        products.Add(data.ElementAt(i + 1));
                    }
                }
            }
            else if (totalRow == 1)
            {
                products.Add(data.ElementAt(0));
            }
            foreach (var item in products)
            {
                var productDetails = await GetDetailsByProductId(item.Id);
                item.ProductDetailViewModels = productDetails;
                foreach (var productDetail in productDetails)
                {
                    item.Stock += productDetail.Stock;
                }
            }

            var page = new PagedResult<ProductViewModel>()
            {
                Items = products,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow
            };
            return page;
        }
        public async Task<ApiResult<bool>> UpdateDetail(ProductDetailUpdateRequest request, Guid userInfoId)
        {
            var productDetail = await _context.ProductDetails.FindAsync(request.Id);
            if (productDetail == null)
            {
                return new ApiErrorResult<bool>($"Sản phẩm id:{request.Id} không tồn tại");
            }
            productDetail.Name = request.ProductDetailName;
            productDetail.Detail = request.Detail;
            productDetail.Width = request.Width;
            productDetail.Length = request.Length;
            productDetail.Stock = request.Stock;
            productDetail.Price = request.Price;
            productDetail.OriginalPrice = request.OriginalPrice;
            _context.ProductDetails.Update(productDetail);
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateProductDetail);
            var censor = new Censor()
            {
                ActionId = action.Id,
                UserInfoId = userInfoId,
                ProductId = productDetail.ProductId,
                Date = DateTime.UtcNow
            };
            
            await _context.Censors.AddAsync(censor);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }
        public async Task<bool> IsMyProduct(Guid userId, int productId)
        {
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProduct);
            var query = from p in _context.Products
                        join cen in _context.Censors on p.Id equals cen.ProductId
                        where cen.UserInfoId == userId && p.Status == Status.Active
                             && cen.ActionId == action.Id && p.Id == productId
                        select new { p };
            if (query.Count() > 0)
                return true;
            else
                return false;
        }
        public async Task<int> AddDetail(ProductDetailCreateRequest request, Guid userInfoId)
        {
            var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProductDetail);
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
                return -1;
            var productDetail = new ProductDetail()
            {
                DateCreated = DateTime.UtcNow,
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                Name = request.ProductDetailName,
                Detail = request.Detail,
                Length = request.Length,
                Width = request.Width,
                ProductId = request.ProductId
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
                return -1;
            }
            await _context.ProductDetails.AddAsync(productDetail);
            var result = await _context.SaveChangesAsync();
            if (result < 1)
                return -1;
            var censors = new Censor()
            {
                ActionId = action.Id,
                UserInfoId = userInfoId,
                Date = DateTime.UtcNow,
                ProductId = product.Id
            };
            await _context.Censors.AddAsync(censors);
            return productDetail.Id;
        }
        public async Task<bool> DeleteDetail(int productDetailId, Guid userId)
        {
            var detail = await _context.ProductDetails.FindAsync(productDetailId);
            if (detail == null)
                return false;

            _context.ProductDetails.Remove(detail);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var Imgages = _context.ProductImages.Where(p => p.ProductDetailId == productDetailId);
                foreach (var image in Imgages)
                {
                    int isDeleteSuccess = _storageService.DeleteFile(image.ImagePath);
                    if (isDeleteSuccess == -1)
                        return false;
                }

                var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.DeleteDetail);
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    Date = DateTime.UtcNow,
                    ProductId = detail.ProductId,
                    UserInfoId = userId,
                };
                _context.Censors.Add(censor);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }




        //----------------Images-------
        // No Done
        public async Task<bool> DeleteImage(int imageId, Guid userId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                return false;

            var productDetail = await _context.ProductDetails.FindAsync(productImage.ProductDetailId);
            if (productDetail == null)
                return false;
            var listProductImages = from pd in _context.ProductDetails
                                    join i in _context.ProductImages on pd.Id equals i.ProductDetailId
                                    select new { i };
            if (listProductImages.Count() <= 1)
                return false;
            
            int isDeleteSuccess = _storageService.DeleteFile(productImage.ImagePath);
            if (isDeleteSuccess == -1)
                return false;
            _context.ProductImages.Remove(productImage);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.DeleteImage);
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    Date = DateTime.UtcNow,
                    ProductId = productDetail.ProductId,
                    UserInfoId = userId,
                };
                _context.Censors.Add(censor);
                return true;
            }
               
            return false;
        }
        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new eRentException($"Cannot find a product: { imageId}");

            var productViewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                ImagePath = image.ImagePath,
                FileSize = image.FileSize,
                DateCreated = image.DateCreated,
                ProductDetailId = image.ProductDetailId,
                Id = image.Id,
                IsDefault = image.IsDefault
            };
            return productViewModel;
        }
        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eRentException($"Cannot find a product: { productId}");
            var productDetail = await _context.ProductDetails.FirstOrDefaultAsync(x => x.ProductId == productId);
            return await _context.ProductImages.Where(x => x.ProductDetailId == productDetail.Id)
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
        }
        public async Task<List<ProductImageViewModel>> GetListImageByProductDetailId(int productDetailId)
        {
            var productDetail = await _context.ProductDetails.FindAsync(productDetailId);
            return await _context.ProductImages.Where(x => x.ProductDetailId == productDetail.Id)
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
        }
        public async Task<ApiResult<string>> AddImage(ProductImageCreateRequest request, Guid userId)
        {
            var product = await _context.Products.FindAsync(request.ProductDetailId);
            if (product == null)
                return new ApiErrorResult<string>($"Sản phẩm id:{request.ProductDetailId} không tồn tại");
            var productImage = new ProductImage()
            {
                Caption = request.Caption == null ? "Non-caption" : request.Caption,
                DateCreated = DateTime.UtcNow,
                ProductDetailId = request.ProductDetailId
            };
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            var result = await _context.SaveChangesAsync();
            if(result>0)
            {
                var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateImage);
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    Date = DateTime.UtcNow,
                    ProductId = product.Id,
                    UserInfoId = userId,
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
                int isDeleteSuccess =_storageService.DeleteFile(productImage.ImagePath);
                if (isDeleteSuccess == -1)
                    return new ApiErrorResult<string>($"Sửa ảnh thất bại, vui lòng thử lại sau vài giây.");
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var action = await _context.UserActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateImage);
                var detail = await _context.ProductDetails.FindAsync(productImage.ProductDetailId);
                var censor = new Censor()
                {
                    ActionId = action.Id,
                    Date = DateTime.UtcNow,
                    ProductId = detail.ProductId,
                    UserInfoId = userId,
                };
                _context.Censors.Add(censor);
                return new ApiSuccessResult<string>($"Thêm ảnh thành công");
            }
            return new ApiErrorResult<string>("Thêm ảnh thất bại");
        }





        //--------FILE--------
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public async Task<ProductDetailViewModel> GetProductDetailById(int productDetailId)
        {
            var productDetail = await _context.ProductDetails.FindAsync(productDetailId);
            if (productDetail == null)
                return null;
            var viewModel = new ProductDetailViewModel()
            {
                Id = productDetail.Id,
                DateCreated = productDetail.DateCreated,
                Detail = productDetail.Detail,
                Images = await GetListImageByProductDetailId(productDetailId),
                Length = productDetail.Length,
                Width = productDetail.Width,
                OriginalPrice = productDetail.OriginalPrice,
                ProductDetailName =productDetail.Name,
                Price = productDetail.Price,
                Stock = productDetail.Stock,
                ProductId = productDetail.ProductId
            };
            return viewModel;
        }

        
    }
}
