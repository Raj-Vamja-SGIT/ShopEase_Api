using ShopEase.Common.Helpers;
using ShopEase.Model.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Data.DBRepository.UserProfile
{
    public interface IUserProfileRepository
    {
        #region
        Task<UsersModel> GetUserProfileDetailsByUserId(int userId);
        Task<BaseApiResponse> UpdateUserProfile(UsersModel user);
        #endregion
    }
}
