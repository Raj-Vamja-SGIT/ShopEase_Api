using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Model.ViewModels.Login
{    
    public class LoginResponseModel
    {       
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int RoleId { get; set; }

    }

}
