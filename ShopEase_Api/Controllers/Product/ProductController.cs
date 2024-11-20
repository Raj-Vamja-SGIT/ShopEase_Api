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

        #region Post
        [HttpGet("GetProducts")]
        public async Task<ApiResponse<ProductModel>> GetProducts(int roleId)
        {
            ApiResponse<ProductModel> response = new ApiResponse<ProductModel> { Data = new List<ProductModel>() };
            try
            {
                var results = await _productService.GetProductList(roleId);
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

        #endregion

        #region Post
        [HttpPost("AddProduct")]
        public async Task<BaseApiResponse> AddProduct([FromForm] ProductModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (product.PrimaryImageFile != null && product.PrimaryImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine("wwwroot", "Documents", "Product", "Thumbnail");
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(product.PrimaryImageFile.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        Directory.CreateDirectory(uploadsFolder);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await product.PrimaryImageFile.CopyToAsync(stream);
                        }
                        product.PrimaryImageUrl = fileName;
                    }
                    var result = await _productService.AddProduct(product);
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
