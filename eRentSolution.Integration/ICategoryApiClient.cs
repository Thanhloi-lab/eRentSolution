﻿using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Integration
{
    public interface ICategoryApiClient
    {
        Task<ApiResult<List<CategoryViewModel>>> GetAll(string tokenName);
        Task<ApiResult<CategoryViewModel>> GetById(int id, string tokenName);
        Task<ApiResult<List<CategoryViewModel>>> GetAllCategoryByProductId(int productId, string tokenName);
        Task<ApiResult<string>> UpdateImage(CategoryImageUpdateRequest request, string tokenName);
        Task<ApiResult<PagedResult<CategoryViewModel>>> GetPagings(GetCategoryPagingRequest request, string tokenName);
    }
}
