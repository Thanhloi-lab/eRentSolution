﻿using eRentSolution.Data.EF;
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
        public async Task<int> Create(ProductCreateRequest request, Guid userInfoId)
        {
            var action = await _context.AdminActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.CreateProduct);
            var product = new Product()
            {
                Name = request.Name,
                DateCreated = DateTime.UtcNow,
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
                        DateCreated = DateTime.UtcNow,
                        Price = request.Price,
                        OriginalPrice = request.OriginalPrice,
                        Stock = request.Stock,
                        Name = request.SubProductName,
                        IsThumbnail =true,
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
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.UtcNow,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }
        public async Task<bool> Delete(int productId, Guid userInfoId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new eRentException($"Cannot find a product: { product.Id}");
            }
            // Lay anh
            var Imgages = _context.ProductImages.Where(p => p.ProductDetailId == productId);
            foreach (var image in Imgages)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

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

            var action = await _context.AdminActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.HideProduct);
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
            var action = await _context.AdminActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdatePriceProduct);
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
            var action = await _context.AdminActions.FirstOrDefaultAsync(x => x.ActionName == SystemConstant.ActionSettings.UpdateStockProduct);
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
        public async Task<bool> Update(ProductUpdateRequest request, Guid userInfoId, int productId)
        {
            var product = await _context.Products.FindAsync(productId);
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
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductDetailId == request.Id);
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
                UserInfoId = userInfoId,
                ProductId = product.Id,
                Date = DateTime.UtcNow
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
                Details = x.p.Details,
                Name = x.p.Name,
                SeoAlias = x.p.SeoAlias,
                SeoDescription = x.p.SeoDescription,
                SeoTitle = x.p.SeoTitle,
                ViewCount = x.p.ViewCount,
                Status = x.p.Status
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
                TotalRecords = products.Count
            };
            return page;
        }
        public async Task<ProductViewModel> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new eRentException($"Cannot find a product: { product.Id}");

            

            var categories = await (from c in _context.Categories
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                                    where pic.ProductId == id
                                    select c.Name).ToListAsync();

            var productDetails = await GetDetailsByProductId(product.Id);

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
                ProductDetailViewModels = productDetails,
                Status = product.Status,
                Categories = categories
            };

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
                        select new { pd };
            var productDetail = await query.Select(x => new ProductDetailViewModel()
            {
                Id = x.pd.Id,
                DateCreated = x.pd.DateCreated,
                IsThumbnail = x.pd.IsThumbnail,
                OriginalPrice = x.pd.OriginalPrice,
                Price = x.pd.Price,
                ProductDetailName = x.pd.Name,
                Stock = x.pd.Stock, 
            }).ToListAsync();
            return productDetail;
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

                        where p.IsFeatured == true
                        select new { p, pd };


            var products = await query.OrderByDescending(x => x.p.DateCreated)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
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
                    Status = x.p.Status
                }).ToListAsync();

            foreach (var item in products)
            {
                var productDetails = await GetDetailsByProductId(item.Id);
                item.ProductDetailViewModels = productDetails;
                foreach (var productDetail in productDetails)
                {
                    item.Stock += productDetail.Stock;
                }
            }

            int totalRow = await query.CountAsync();
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

            var products = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductViewModel()
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
                    Status = x.p.Status
                }).Distinct().ToListAsync();

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

        //----------------Images-------
        // No Done
        public async Task<int> RemoveImages(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) throw new eRentException($"Cannot find a img : {imageId}");

            var productDetail = await _context.ProductDetails.FindAsync(productImage.ProductDetailId);
            if (productDetail == null) throw new eRentException($"Cannot find a product: { productImage.ProductDetailId}");

            if (productImage.IsDefault == true && productDetail.ProductImages.Count != 1)
            {
                var ortherImage = _context.ProductImages.FirstOrDefault(x => x.ProductDetailId == productImage.ProductDetailId && x.Id != imageId);
                ortherImage.IsDefault = true;
                _context.ProductImages.Update(ortherImage);
            }
            await _storageService.DeleteFileAsync(productImage.ImagePath);
            productDetail.ProductImages.Remove(productImage);


            return await _context.SaveChangesAsync();
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
                IsDefault = image.IsDefault,
                ProductDetailId = image.ProductDetailId,
                Id = image.Id,
                SortOrder = image.SortOrder
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
                        SortOrder = i.SortOrder
                    }).ToListAsync();
        }
        // No Done
        public async Task<int> AddImages(ProductImageCreateRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null) throw new eRentException($"Cannot find a product: { request.ProductId }");
            var productImage = new ProductImage()
            {
                Caption = request.Caption == null ? "Non-caption" : request.Caption,
                DateCreated = DateTime.UtcNow,
                IsDefault = request.IsDefault,
                SortOrder = request.SortOrder,
                ProductDetailId = request.ProductId
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

        //--------FILE--------
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        
    }
}
