using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopEase.Common.Helpers;
using ShopEase.Model.ViewModels.Product;
using ShopEase.Model.ViewModels.User;
using ShopEase.Services.Product;
using ShopEase.Services.UserProfile;

namespace ShopEase_Api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        #region Fields
        private IProductService _productService;
        #endregion

        #region Constructor
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region Get
        [HttpGet("GetProducts")]
        public async Task<ApiResponse<ProductModel>> GetProducts(int Id, string searchTerm = null)
        {
            ApiResponse<ProductModel> response = new ApiResponse<ProductModel> { Data = new List<ProductModel>() };
            try
            {
                var results = await _productService.GetProductList(Id, searchTerm);
                if (results != null)
                {
                    response.Data = results;
                    response.Success = true;
                    response.Message = Messages.GetProductsSuccess;
                }
            }
            catch (Exception ex)
            {
                response = new ApiResponse<ProductModel>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
            return response;
        }

        [HttpGet("GetProductDetails")]
        public async Task<ApiPostResponse<ProductModel>> GetUserProfile(int productId)
        {
            ApiPostResponse<ProductModel> response = new ApiPostResponse<ProductModel>();
            ProductModel productResponseModel = new ProductModel();

            ProductModel result = await _productService.GetProductDetaiById(productId);

            if (result != null)
            {
                ProductModel model = new ProductModel();
                {
                    model.ProductId = result.ProductId;
                    model.ProductName = result.ProductName;
                    model.ProductDescription = result.ProductDescription;
                    model.Price = result.Price;
                    model.Discount = result.Discount;
                    model.StockStatus = result.StockStatus;
                    model.SKU = result.SKU;
                    model.Rating = result.Rating;
                    model.Brand = result.Brand;
                    model.Category = result.Category;
                };

                productResponseModel = model;
                response.Success = true;
                response.Message = Messages.GetProductSuccess;
            }
            else
            {
                response.Success = false;
                response.Message = Messages.GetUserProfileError;
            }
            response.Data = productResponseModel;
            return response;
        }

        [HttpGet("GetBrands")]
        public async Task<ApiResponse<BrandModel>> GetBrands(int categoryId)
        {
            ApiResponse<BrandModel> response = new ApiResponse<BrandModel> { Data = new List<BrandModel>() };
            try
            {
                var results = await _productService.GetBrandList(categoryId);
                if (results != null)
                {
                    response.Data = results;
                    response.Success = true;
                    response.Message = Messages.GetBrandsSuccess;
                }
            }
            catch (Exception ex)
            {
                response = new ApiResponse<BrandModel>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
            return response;
        }

        [HttpGet("GetCategories")]
        public async Task<ApiResponse<CategoryModel>> GetCategories()
        {
            ApiResponse<CategoryModel> response = new ApiResponse<CategoryModel> { Data = new List<CategoryModel>() };
            try
            {
                var results = await _productService.GetCategoryList();
                if (results != null)
                {
                    response.Data = results;
                    response.Success = true;
                    response.Message = Messages.GetCategoriesSuccess;
                }
            }
            catch (Exception ex)
            {
                response = new ApiResponse<CategoryModel>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
            return response;
        }

        [HttpGet("GetProductImages")]
        public async Task<ApiResponse<ProductImageViewModel>> GetProductImages(int productId)
        {
            ApiResponse<ProductImageViewModel> response = new ApiResponse<ProductImageViewModel> { Data = new List<ProductImageViewModel>() };
            try
            {
                var results = await _productService.GetProductImages(productId);
                if (results != null)
                {
                    response.Data = results;
                    response.Success = true;
                    response.Message = Messages.GetProductsSuccess;
                }
            }
            catch (Exception ex)
            {
                response = new ApiResponse<ProductImageViewModel>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
            return response;
        }

        #endregion

        #region Post
        [HttpPost("AddUpdateProduct")]
        public async Task<BaseApiResponse> AddUpdateProduct([FromForm] ProductModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _productService.AddUpdateProduct(product);
                    return result;
                }
                else
                {
                    return new ApiResponse<BaseApiResponse>
                    {
                        Success = false,
                        Message = Messages.AddProductError,
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<BaseApiResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPost("AddProductImages")]
        public async Task<BaseApiResponse> AddProductImages([FromForm] ProductImageModel productImage)
        {
            if (productImage.ImageFiles != null && productImage.ImageFiles.Any())
            {
                var uploadsFolder = Path.Combine("wwwroot", "Documents", "Product", "Images");
                Directory.CreateDirectory(uploadsFolder);

                // List to store uploaded image URLs
                var imageUrls = new List<string>();

                foreach (var imageFile in productImage.ImageFiles)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Add the file name to the list of image URLs
                        imageUrls.Add(fileName);
                    }
                }

                // Convert the list of image URLs into a comma-separated string
                var commaSeparatedImageUrls = string.Join(",", imageUrls);

                productImage = new ProductImageModel()
                {
                    ProductId = productImage.ProductId,
                    ImageUrls = new List<string> { commaSeparatedImageUrls }
                };

                var result = await _productService.AddProductImages(productImage);
                return result;
            }
            else
            {
                return new ApiResponse<BaseApiResponse>
                {
                    Success = false,
                    Message = Messages.AddProductImageError,
                };
            }
        }

        [HttpPost("DeleteProductImage")]
        public async Task<BaseApiResponse> DeleteProductImage(long imageId)
        {
            try
            {
                if (imageId > 0)
                {
                    var result = await _productService.DeleteProductImage(imageId);
                    return result;
                }
                else
                {
                    return new ApiResponse<BaseApiResponse>
                    {
                        Success = false,
                        Message = Messages.AddProductError,
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<BaseApiResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        #endregion
    }
}
