using Dapper;
using Microsoft.Extensions.Options;
using ShopEase.Common.Helpers;
using ShopEase.Model.Config;
using ShopEase.Model.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Data.DBRepository.UserProfile
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {

        #region Constructor
        public UserProfileRepository(IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
        }

        #endregion

        #region Get
        public async Task<UsersModel> GetUserProfileDetailsByUserId(int userId)
        {
            BaseApiResponse response = new BaseApiResponse();

            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                return await QueryFirstOrDefaultAsync<UsersModel>("sp_GetUserProfileDetailsById", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return null;
            }
        }
        #endregion

        #region Post
        public async Task<BaseApiResponse> UpdateUserProfile(UsersModel user)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", user.UserId);
                param.Add("@UserName", user.UserName);
                param.Add("@UserEmail", user.UserEmail);
                param.Add("@Password", user.Password);
                param.Add("@DOB", user.DOB);
                param.Add("@Gender", user.Gender);
                param.Add("@Avatar", user.Avatar);
                var result = await QueryFirstOrDefaultAsync<int>("sp_UpdateUserProfile", param, commandType: CommandType.StoredProcedure);

                if (result != null && result > 0)
                {
                    response.Success = true;
                    response.Message = Messages.UpdateUserProfileSuccess;
                }
                else
                {
                    response.Success = false;
                    response.Message = Messages.UpdateUserProfileError;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion
    }
}
