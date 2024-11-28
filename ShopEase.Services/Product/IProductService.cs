using ShopEase.Common.Helpers;
using ShopEase.Model.ViewModels.Product;
using ShopEase.Model.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Services.Product
{
    public interface IProductService
    {
        #region Post
        Task<BaseApiResponse> AddUpdateProduct(ProductModel product);
        Task<BaseApiResponse> AddProductImages(ProductImageModel productImages);
        Task<BaseApiResponse> DeleteProductImage(long imageId);
        Task<BaseApiResponse> UpdateImageOrder(string updatedOrdre);


        #endregion
        #region Get
        Task<List<ProductModel>> GetProductList(int Id, string searchTerm = null);
        Task<List<CategoryModel>> GetCategoryList();
        Task<List<BrandModel>> GetBrandList(int categoryId);
        Task<ProductModel> GetProductDetaiById(int productId);
        Task<List<ProductImageViewModel>> GetProductImages(int productId);

        #endregion
    }
}
