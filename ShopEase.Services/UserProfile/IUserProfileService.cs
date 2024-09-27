using ShopEase.Common.Helpers;
using ShopEase.Model.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Services.UserProfile
{
    public interface IUserProfileService
    {
        #region Get
        Task<UsersModel> GetUserProfileByUserId(int userId);
        #endregion
        #region Post
        Task<BaseApiResponse> UpdateUserProfile(UsersModel user);
        #endregion
    }
}
