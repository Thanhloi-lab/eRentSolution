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
        public async Task<int> Create(ProductCreateRequest request, int userInfoId)
        {
            var product = new Product()
            {
                Name = request.Name,
                DateCreated = DateTime.Now,
                Description = request.Description,
                Details = request.Details,

                ViewCount = 0,
                SeoAlias = request.SeoAlias,
                SeoDescription = request.SeoDescription,
                SeoTitle = request.SeoTitle,
                ProductDetails = new List<ProductDetail>()
                {
                    new ProductDetail()
                    {
                        DateCreated = DateTime.Now,
                        Price = request.Price,
                        OriginalPrice = request.OriginalPrice,
                        Stock = request.Stock,
                        Name = request.SubProductName,
                        IsThumbnail =true
                    }
                }
            };
            // Luu Anh
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }
            await _context.Products.AddAsync(product);

            var action = await _context.AdminActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProduct);
            var censor = new Censor()
            {
                ActionId = action.Id,
                PersonId = userInfoId,
                ProductId = product.Id
            };
            await _context.Censors.AddAsync(censor);

            return await _context.SaveChangesAsync();
        }
        public async Task<bool> Delete(int productId, int userInfoId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new eRentException($"Cannot find a product: { product.Id}");
            }
            // Lay anh
            var Imgages = _context.ProductImages.Where(p => p.ProductId == productId);
            foreach (var image in Imgages)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }
            
            _context.Products.Remove(product);

            var action = await _context.AdminActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.DeleteProduct);
            var censor = new Censor()
            {
                ActionId = action.Id,
                PersonId = userInfoId,
                ProductId = productId
            };
            await _context.Censors.AddAsync(censor);
            var result = await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdatePrice(int productDetailId ,decimal newPrice, int userInfoId)
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
            var action = await _context.AdminActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdatePriceProduct);
            var censor = new Censor()
            {
                ActionId = action.Id,
                PersonId = userInfoId,
                ProductId = product.Id
            };
            await _context.Censors.AddAsync(censor);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateStock(int productDetailId, int addedQuantity, int userInfoId)
        {
            var productDetail = await _context.ProductDetails.FindAsync(productDetailId);
            if (productDetail == null)
            {
                return false;
            }
            productDetail.Stock += addedQuantity;

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productDetail.ProductId);
            var action = await _context.AdminActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateStockProduct);
            var censor = new Censor()
            {
                ActionId = action.Id,
                PersonId = userInfoId,
                ProductId = product.Id
            };
            await _context.Censors.AddAsync(censor);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(ProductUpdateRequest request, int userInfoId)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                return false;
            }
            product.Name = request.Name;
            product.Description = request.Description;
            product.Details = request.Details;
            product.SeoAlias = request.SeoAlias;
            product.SeoTitle = request.SeoTitle;
            product.SeoDescription = request.SeoDescription;
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }
            var action = await _context.AdminActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateProduct);
            var censor = new Censor()
            {
                ActionId = action.Id,
                PersonId = userInfoId,
                ProductId = product.Id
            };
            await _context.Censors.AddAsync(censor);
            await _context.SaveChangesAsync();
            return true;
        }
        //Fixed
        //Lay theo categoryid nen khong can left join
        //Phai distinct vi 1 san pham co the co nhieu category nen no se lay trung
        //public async Task<PagedResult<ProductViewModel>> GetAllPagingByCategoryId(GetProductPagingByCategoryIdRequest request)
        //{
        //    var query = from p in _context.Products
        //                join pic in _context.ProductInCategories on p.Id equals pic.ProductId
        //                join c in _context.Categories on pic.CategoryId equals c.Id
        //                where c.Id == request.CategoryId
        //                select new { p, c };

        //    var products = await query.Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).Select(x => new ProductViewModel()
        //    {
        //        Id = x.p.Id,
        //        DateCreated = x.p.DateCreated,
        //        Description = x.p.Description,
        //        Details = x.p.Details,
        //        Name = x.p.Name,
        //        OriginalPrice = x.p.OriginalPrice,
        //        Price = x.p.Price,
        //        SeoAlias = x.p.SeoAlias,
        //        SeoDescription = x.p.SeoDescription,
        //        SeoTitle = x.p.SeoTitle,
        //        Stock = x.p.Stock,
        //        ViewCount = x.p.ViewCount
        //    }).Distinct().ToListAsync();
        //    var page = new PagedResult<ProductViewModel>()
        //    {
        //        Items = products,
        //        PageIndex = request.PageIndex,
        //        PageSize = request.PageSize,
        //        TotalRecords = products.Count
        //    };
        //    return page;
        //}
        //fixed co ham get theo category roi nen khong can lay theo category nua
        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            var query = from p in _context.Products
                            //join pd in _context.ProductDetails on p.Id equals pd.Id
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                                 //&& pd.IsThumbnail == true
                        select new { p, c };//, pd};

            if (request.Keyword != null)
            {
                query = query.Where(x => x.p.Name.Contains(request.Keyword));
            }
            var products = await query.Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                DateCreated = x.p.DateCreated,
                Description = x.p.Description,
                Details = x.p.Details,
                Name = x.p.Name,
                SeoAlias = x.p.SeoAlias,
                SeoDescription = x.p.SeoDescription,
                SeoTitle = x.p.SeoTitle,
                ViewCount = x.p.ViewCount,
            }).ToListAsync();
            foreach (var item in products)
            {
                item.ProductDetailViewModels = await GetDetailsByProductId(item.Id);
            }
            var page = new PagedResult<ProductViewModel>()
            {
                Items = products,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = products.Count
            };
            return page;
        }
        public async Task<ProductViewModel> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new eRentException($"Cannot find a product: { product.Id}");
            }
            var productViewModel = new ProductViewModel()
            {
                DateCreated = product.DateCreated,
                Description = product.Description,
                Details = product.Details,
                Id = product.Id,
                Name = product.Name,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                ViewCount = product.ViewCount,
                ProductDetailViewModels = await GetDetailsByProductId(id),
            };
            return productViewModel;
        }
        public async Task<List<ProductDetailViewModel>> GetDetailsByProductId(int productId)
        {
            var query =from pd in _context.ProductDetails
                        join p in _context.Products on pd.ProductId equals p.Id
                        select new { pd };
            var productDetail = await query.Select(x => new ProductDetailViewModel()
            {
                Id = x.pd.Id,
                DateCreated = x.pd.DateCreated,
                IsThumbnail = x.pd.IsThumbnail,
                OriginalPrice = x.pd.OriginalPrice,
                Price = x.pd.Price,
                ProductDetailName = x.pd.Name,
                Stock = x.pd.Stock
            }).ToListAsync();
            return productDetail;
        }


        //----------------Images-------
        // No Done
        public async Task<int> RemoveImages(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) throw new eRentException($"Cannot find a img : {imageId}");

            var product = await _context.Products.FindAsync(productImage.ProductId);
            if (product == null) throw new eRentException($"Cannot find a product: { productImage.ProductId}");

            if (productImage.IsDefault == true && product.ProductImages.Count != 1)
            {
                var ortherImage = _context.ProductImages.FirstOrDefault(x => x.ProductId == productImage.ProductId && x.Id != imageId);
                ortherImage.IsDefault = true;
                _context.ProductImages.Update(ortherImage);
            }
            await _storageService.DeleteFileAsync(productImage.ImagePath);
            product.ProductImages.Remove(productImage);


            return await _context.SaveChangesAsync();
        }
        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new eRentException($"Cannot find a product: { image.Id}");

            var productViewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                ImagePath = image.ImagePath,
                FileSize = image.FileSize,
                DateCreated = image.DateCreated,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                Id = image.Id,
                SortOrder = image.SortOrder
            };
            return productViewModel;
        }
        // No Done
        public async Task<int> AddImages(ProductImageCreateRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null) throw new eRentException($"Cannot find a product: { request.ProductId }");
            var productImage = new ProductImage()
            {
                Caption = request.Caption == null ? "Non-caption" : request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                SortOrder = request.SortOrder,
                ProductId = request.ProductId
            };
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }
        // No Done
        public async Task<int> UpdateImages( ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(request.ImageId);
            if (productImage == null) throw new eRentException($"Cannot find a image: {request.ImageId}");
            productImage.Caption = request.Caption;
            productImage.SortOrder = request.SortOrder;
            if (request.imageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.imageFile);
                productImage.FileSize = request.imageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }
        // No Done
        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new eRentException($"Cannot find a product: { product.Id}");
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                    .Select(i => new ProductImageViewModel()
                    {
                        Caption = i.Caption,
                        ImagePath = i.ImagePath,
                        FileSize = i.FileSize,
                        DateCreated = i.DateCreated,
                        IsDefault = i.IsDefault,
                        ProductId = i.ProductId,
                        Id = i.Id,
                        SortOrder = i.SortOrder
                    }).ToListAsync();
        }

        //--------FILE--------
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<ProductViewModel>> GetFeaturedProducts(GetProductPagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductViewModel>> GetLastestProducts(int take)
        {
            throw new NotImplementedException();
        }
    }
}
