using ShopEase.Common.Helpers;
using ShopEase.Data.DBRepository.Product;
using ShopEase.Model.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Services.Product
{
    public class ProductService : IProductService
    {
        #region Fields
        public readonly IProductRepository _productRepository;
        #endregion

        #region Construtor
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        #endregion

        #region Post
        public async Task<BaseApiResponse> AddProduct(ProductModel product)
        {
           return await _productRepository.AddProduct(product); 
        }
        #endregion
        #region Get
        public async Task<List<ProductModel>> GetProductList(int roleId)
        {
            return await _productRepository.GetProductList(roleId);
        }
        #endregion
    }
}
