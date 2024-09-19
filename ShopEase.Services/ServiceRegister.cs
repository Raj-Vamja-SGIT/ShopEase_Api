using ShopEase.Service.Services.Account;
using System;
using System.Collections.Generic;

namespace ShopEase.Service
{
    public static class ServiceRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var serviceDictonary = new Dictionary<Type, Type>
            {
                { typeof(ILoginService), typeof(LoginService) }
            };
            return serviceDictonary;
        }
    }
}
