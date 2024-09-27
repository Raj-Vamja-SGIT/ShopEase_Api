using ShopEase.Common.Helpers;
using ShopEase.Data.DBRepository.Account;
using ShopEase.Data.DBRepository.UserProfile;
using ShopEase.Model.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Services.UserProfile
{
    public class UserProfileService : IUserProfileService
    {
        #region Fields
            public readonly IUserProfileRepository _userProfileRepository;
        #endregion

        #region Construtor
        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }
        #endregion

        #region Get
        public async Task<UsersModel> GetUserProfileByUserId(int userId)
        {
            return await _userProfileRepository.GetUserProfileDetailsByUserId(userId);
        }
        #endregion

        #region Post
        public async Task<BaseApiResponse> UpdateUserProfile(UsersModel user)
        {
            return await _userProfileRepository.UpdateUserProfile(user);
        }
        #endregion
    }
}
