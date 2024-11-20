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

namespace ShopEase.Data.DBRepository.Admin
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {

        #region Constructor
        public AdminRepository(IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
        }

        #endregion

        #region Get
        public async Task<List<UserListModel>> GetUserList()
        {
            try
            {
                var userList = await QueryAsync<UserListModel>("sp_GetUserList", commandType: CommandType.StoredProcedure);

                if (userList != null && userList.Any())
                {
                    return userList.ToList();
                }
                else
                {
                    return new List<UserListModel>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DashboardData>> GetDashboardData()
        {
            try
            {
                var result = await QueryAsync<DashboardData>("sp_GetDashboardData", commandType: CommandType.StoredProcedure);
                if (result != null)
                {
                    return result.ToList();
                }
                else
                {
                    return new List<DashboardData>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Post
        public async Task<BaseApiResponse> AddUser(UsersModel user)
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
                param.Add("@Role", user.Role);
                param.Add("@Avatar", user.Avatar);
                var result = await QueryFirstOrDefaultAsync<int>("sp_AddShopEaseUser", param, commandType: CommandType.StoredProcedure);

                if (result != null && result >= 1)
                {
                    response.Success = true;
                    response.Message = result == 1 ? Messages.UpdateUserSuccess : Messages.AddUserSuccess;
                }
                else
                {
                    response.Success = false;
                    response.Message = Messages.AddUserError;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<BaseApiResponse> DeleteUser(int userId)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                var result = await QueryFirstOrDefaultAsync<int>("sp_DeleteShopEaseUser", param, commandType: CommandType.StoredProcedure);

                if (result != null && result > 0)
                {
                    response.Success = true;
                    response.Message = Messages.DeleteUserSuccess;
                }
                else
                {
                    response.Success = false;
                    response.Message = Messages.DeleteUserError;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return response;
        }

        public async Task<BaseApiResponse> DeleteMultiUser(SelectedUserModel users)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserIds", users.userIds);
                var result = await QueryFirstOrDefaultAsync<int>("sp_DeleteMultiShopEaseUser", param, commandType: CommandType.StoredProcedure);

                if (result != null && result > 0)
                {
                    response.Success = true;
                    response.Message = Messages.DeleteMultiUserSuccess;
                }
                else
                {
                    response.Success = false;
                    response.Message = Messages.DeleteUserError;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        #endregion
    }
}
