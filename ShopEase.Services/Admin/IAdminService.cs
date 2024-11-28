using ShopEase.Common.Helpers;
using ShopEase.Data.DBRepository.Admin;
using ShopEase.Data.DBRepository.UserProfile;
using ShopEase.Model.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Services.Admin
{
    public interface IAdminService
    {
        #region Get
        Task<List<UserListModel>> GetUsers();
        Task<List<DashboardData>> GetDashboardData();
        #endregion

        #region Post
        Task<BaseApiResponse> AddUser(UsersModel users);
        Task<BaseApiResponse> DeleteUser(int userId);
        Task<BaseApiResponse> DeleteMultiUser(SelectedUserModel users);
        #endregion
    }
}
