using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Model.ViewModels.User
{
    public class UsersModel
    {
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? Password { get; set; }
        public long RoleId { get; set; }
        public DateTime? DOB { get; set; }
        public string? Gender { get; set; } 
        public long? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Avatar { get; set; }
        public IFormFile? AvatarFile { get; set; }
    }
}