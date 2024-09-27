using ShopEase.Common.Helpers;
using ShopEase.Model.ViewModels.Login;
using ShopEase.Model.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Service.Services.Account
{
    public interface ILoginService
    {
        #region Post
        Task<LoginResponseModel> LoginUser(LoginRequestModel model);
        Task<BaseApiResponse> RegisterUser(RegisterUserRequestModel model);
        #endregion


        Task<BaseApiResponse> ForgotPassword(ForgotPasswordRequestModel model);

    }
}
