using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Model.ViewModels.Product
{
    public class ProductModel
    {
        public long ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal Price { get; set; } 
        public decimal Discount { get; set; } 
        public string StockStatus { get; set; } 
        public string? SKU { get; set; }
        public int? Rating { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; } 
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } 
        public bool IsActive { get; set; } = true;
        public string? PrimaryImageUrl { get; set; }
        public IFormFile? PrimaryImageFile { get; set; }
    }
}
