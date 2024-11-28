using ShopEase.Common.Enum;
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
        public async Task<BaseApiResponse> AddUpdateProduct(ProductModel product)
        {
           return await _productRepository.AddUpdateProduct(product); 
        }
        public async Task<BaseApiResponse> AddProductImages(ProductImageModel productImages)
        {
            return await _productRepository.AddProductImages(productImages);
        }

        public async Task<BaseApiResponse> DeleteProductImage(long imageId)
        {
            return await _productRepository.DeleteProductImage(imageId);
        }

        public async Task<BaseApiResponse> UpdateImageOrder(string updatedOrdre)
        {
            return await _productRepository.UpdateImageOrder(updatedOrdre);
        }

        #endregion

        #region Get
        public async Task<List<ProductModel>> GetProductList(int Id, string searchTerm = null)
        {
            return await _productRepository.GetProductList(Id, searchTerm);
        }

        public async Task<List<BrandModel>> GetBrandList(int categoryId)
        {
            return await _productRepository.GetBrandList(categoryId);
        }

        public async Task<List<CategoryModel>> GetCategoryList()
        {
            return await _productRepository.GetCategoryList();
        }

        public async Task<ProductModel> GetProductDetaiById(int productId)
        {
            return await _productRepository.GetProductDetaiById(productId);
        }

        public async Task<List<ProductImageViewModel>> GetProductImages(int productId)
        {
            return await _productRepository.GetProductImages(productId);
        }

        #endregion
    }
}
