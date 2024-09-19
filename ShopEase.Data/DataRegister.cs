using ShopEase.Data.DBRepository.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Data
{
    public static class DataRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var dataDictionary = new Dictionary<Type, Type>
            {
                { typeof(ILoginRepository), typeof(LoginRepository) }
            };
            return dataDictionary;
        }
    }
}
