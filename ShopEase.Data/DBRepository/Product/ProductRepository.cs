using Azure;
using Dapper;
using Microsoft.Extensions.Options;
using ShopEase.Common.Enum;
using ShopEase.Common.Helpers;
using ShopEase.Model.Config;
using ShopEase.Model.ViewModels.Product;
using ShopEase.Model.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Data.DBRepository.Product
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        #region Constructor
        public ProductRepository(IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
        }

        #endregion
        #region Post
        public async Task<BaseApiResponse> AddUpdateProduct(ProductModel product)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductId", product.ProductId);
                param.Add("@ProductName", product.ProductName);
                param.Add("@ProductDescription", product.ProductDescription);
                param.Add("@Price", product.Price);
                param.Add("@Discount", product.Discount);
                param.Add("@StockQuantity", product.StockStatus);
                param.Add("@SKU", product.SKU);
                param.Add("@BrandId", product.Brand);
                param.Add("@CategoryId", product.Category);
                param.Add("@CreatedBy", product.CreatedBy);
                var result = await QueryFirstOrDefaultAsync<int>("sp_AddUpdateProduct", param, commandType: CommandType.StoredProcedure);

                if (result != null && result > 0)
                {
                    response.Success = true;
                    response.Message = Messages.AddProductSuccess;
                    response.TAID = result;
                }
                else
                {
                    response.Success = false;
                    response.Message = Messages.AddProductError;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<BaseApiResponse> AddProductImages(ProductImageModel productImages)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductID", productImages.ProductId);
                param.Add("@ImageUrls", productImages.ImageUrls[0]);
                var result = await QueryFirstOrDefaultAsync<int>("sp_AddProductImages", param, commandType: CommandType.StoredProcedure);

                if (result != null && result > 0)
                {
                    response.Success = true;
                    response.Message = Messages.AddProductImageSuccess;
                }
                else
                {
                    response.Success = false;
                    response.Message = Messages.AddProductImageError;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<BaseApiResponse> DeleteProductImage(long imageId)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductImageId", imageId);
                var result = await QueryFirstOrDefaultAsync<long>("sp_DeleteProductImage", param, commandType: CommandType.StoredProcedure);

                if (result != null && result > 0)
                {
                    response.Success = true;
                    response.Message = Messages.DeleteProductImageSuccess;
                }
                else
                {
                    response.Success = false;
                    response.Message = Messages.DeleteProductImageError;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        #endregion
        #region Get
        public async Task<List<ProductModel>> GetProductList(int Id, string searchTerm = null)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                param.Add("@SearchTerm", searchTerm);
                var productList = await QueryAsync<ProductModel>("sp_GetProductList", param, commandType: CommandType.StoredProcedure);

                if (productList != null && productList.Any())
                {
                    return productList.ToList();
                }
                else
                {
                    return new List<ProductModel>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BrandModel>> GetBrandList(int categoryId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CategoryId", categoryId);
                var brandList = await QueryAsync<BrandModel>("sp_GetBrandList", param,commandType: CommandType.StoredProcedure);

                if (brandList != null && brandList.Any())
                {
                    return brandList.ToList();
                }
                else
                {
                    return new List<BrandModel>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CategoryModel>> GetCategoryList()
        {
            try
            {
                var categoryList = await QueryAsync<CategoryModel>("sp_GetCategoryList", commandType: CommandType.StoredProcedure);

                if (categoryList != null && categoryList.Any())
                {
                    return categoryList.ToList();
                }
                else
                {
                    return new List<CategoryModel>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductModel> GetProductDetaiById(int productId)
        {
            BaseApiResponse response = new BaseApiResponse();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductId", productId);
                return await QueryFirstOrDefaultAsync<ProductModel>("sp_GetProductDetailsById", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return null;
            }
        }

        public async Task<List<ProductImageViewModel>> GetProductImages(int productId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductID", productId);
                var productImages = await QueryAsync<ProductImageViewModel>("sp_GetProductImagesById", param, commandType: CommandType.StoredProcedure);

                if (productImages != null && productImages.Any())
                {
                    return productImages.ToList();
                }
                else
                {
                    return new List<ProductImageViewModel>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
