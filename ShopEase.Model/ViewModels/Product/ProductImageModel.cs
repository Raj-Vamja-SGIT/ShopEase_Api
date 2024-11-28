using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Model.ViewModels.Product
{
    public class ProductImageModel
    {
        public long ProductId { get; set; }

        public List<IFormFile>? ImageFiles { get; set; }
        public List<string>? ImageUrls { get; set; }
    }

    public class ProductImageViewModel
    {
        public long ImageId { get; set; }
        public long ProductId { get; set; }

        public string? ImageUrls { get; set; }
        public long ImageOrderNumber { get; set; }

    }

    public class ImageOrderUpdateRequestModel
    {
        public string ImageId { get; set; }
        public string ImageOrderNumber { get; set; }
    }

}
