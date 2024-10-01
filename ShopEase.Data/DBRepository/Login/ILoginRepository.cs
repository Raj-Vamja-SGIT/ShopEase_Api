using ShopEase.Common.Helpers;
using ShopEase.Model.ViewModels.Login;
using ShopEase.Model.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Data.DBRepository.Account
{
    public interface ILoginRepository
    {
        #region Get
        Task<UsersModel> GetUserByEmailAsync(string email);
        #endregion

        #region Post
        Task<LoginResponseModel> LoginUser(LoginRequestModel model);
        Task<BaseApiResponse> RegisterUser(RegisterUserRequestModel model);
        Task<BaseApiResponse> ChangeUserPassword(UsersModel user);
        #endregion

    }
}
