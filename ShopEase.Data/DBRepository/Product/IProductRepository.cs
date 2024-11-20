using ShopEase.Common.Helpers;
using ShopEase.Model.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Data.DBRepository.Product
{
    public interface IProductRepository
    {
        #region Post
        Task<BaseApiResponse> AddProduct(ProductModel product);
        #endregion
        #region Get
        Task<List<ProductModel>> GetProductList(int roleId);
        #endregion
    }
}
