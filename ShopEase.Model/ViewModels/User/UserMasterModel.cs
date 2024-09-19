using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Model.ViewModels.User
{
    public class UserMasterModel
    {
        public int? user_id { get; set; }
        public string username { get; set; }
        public int? user_group_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int status { get; set; }
        public string StatusName { get; set; }
        public string ip { get; set; }
        public string image { get; set; }
        public DateTime date_added { get; set; }
        public int? store_id { get; set; }
        public int IsStoreAdmin { get; set; }
        public int Is_ImportPoint { get; set; }
        public int? customer_department_id { get; set; }
        public int? is_department { get; set; }
        public int? is_admin { get; set; }
        public bool IsBlocked { get; set; }
        public int? WrongAttempt { get; set; }
        public string EmailAddress { get; set; }
        public string strMessage { get; set; }
        public string JWTToken { get; set; }
        public string FCMToken { get; set; }
    }
  
}
