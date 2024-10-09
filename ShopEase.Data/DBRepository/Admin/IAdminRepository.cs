using ShopEase.Common.Helpers;
using ShopEase.Model.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Data.DBRepository.Admin
{
    public interface IAdminRepository
    {
        #region Get
        Task<List<UserListModel>> GetUserList();
        Task<List<DashboardData>> GetDashboardData();
        #endregion
        #region Post
        Task<BaseApiResponse> AddUser(UsersModel users);
        Task<BaseApiResponse> DeleteUser(int userId);
        Task<BaseApiResponse> DeleteMultiUser(SelectedUserModel users);
        #endregion
    }
}
