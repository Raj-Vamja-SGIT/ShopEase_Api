using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Model.ViewModels.Login
{
    public class LoginBaseReponseModel
    {        
        public UserProfileModel? UserProfile { get; set; }
        public AccessTokenResponseModel? AccessToken { get; set; }
    }

    public class UserProfileModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? EmailId { get; set; }
    }
    
    public class AccessTokenResponseModel
    {
        public string? Token { get; set; }
        public DateTime ExpiresOnUTC { get; set; }
    }
}
