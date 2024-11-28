using ShopEase.Data;
using ShopEase.Data.DBRepository.Account;
using ShopEase.Data.DBRepository.UserProfile;
using ShopEase.Logger;
using ShopEase.Service;
using ShopEase.Service.Services.JWTAuthentication;
using ShopEase.Services.JWTAuthentication;

namespace ShopEase_Api
{
    public static class RegisterService
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            Configure(services, DataRegister.GetTypes());
            Configure(services, ServiceRegister.GetTypes());
        }


        private static void Configure(IServiceCollection services, Dictionary<Type, Type> types)
        {
            foreach (var type in types)
                services.AddScoped(type.Key, type.Value);
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton<IJWTAuthenticationService, JWTAuthenticationService>();
        }
    }
}
