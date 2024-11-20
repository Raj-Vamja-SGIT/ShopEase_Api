using Dapper;
using Microsoft.Extensions.Options;
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
        public async Task<BaseApiResponse> AddProduct(ProductModel product)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductName", product.ProductName);
                param.Add("@ProductDescription", product.ProductDescription);
                param.Add("@Price", product.Price);
                param.Add("@Discount", product.Discount);
                param.Add("@StockQuantity", product.StockStatus);
                param.Add("@SKU", product.SKU);
                param.Add("@Rating", product.Rating);
                param.Add("@BrandId", product.Brand);
                param.Add("@CategoryId", product.Category);
                param.Add("@IsActive", product.IsActive);
                param.Add("@CreatedDate", product.CreatedDate);
                param.Add("@CreatedBy", product.CreatedBy);
                param.Add("@PrimaryImageUrl", product.PrimaryImageUrl);
                param.Add("@IsActive", product.IsActive);
                var result = await QueryFirstOrDefaultAsync<int>("USP_AddProduct", param, commandType: CommandType.StoredProcedure);

                if (result != null && result > 0)
                {
                    response.Success = true;
                    response.Message = Messages.AddProductSuccess;
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
        #endregion
        #region Get
        public async Task<List<ProductModel>> GetProductList(int roleId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@RoleId", roleId);
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
        #endregion
    }
}
