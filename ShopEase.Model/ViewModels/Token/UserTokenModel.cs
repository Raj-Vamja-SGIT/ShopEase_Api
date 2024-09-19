using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Model.ViewModels.Token
{
    public class UserTokenModel
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string? UserName { get; set; }
        public string? EmailId { get; set; }
        public DateTime TokenValidTo { get; set; }
        public long CompanyId { get; set; }
    }
}
