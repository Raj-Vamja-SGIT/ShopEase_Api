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
        public long ImageId { get; set; }
        public long ProductId { get; set; }
        public string? ImageURL { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
