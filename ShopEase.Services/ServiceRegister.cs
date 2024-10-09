using ShopEase.Model.ViewModels.Login;
using ShopEase.Service.Services.Account;
using ShopEase.Services.Admin;
using ShopEase.Services.UserProfile;
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
                { typeof(ILoginService), typeof(LoginService) },
                { typeof(IUserProfileService), typeof(UserProfileService) },
                { typeof(IAdminService), typeof(AdminService) }
            };
            return serviceDictonary;
        }
    }
}
