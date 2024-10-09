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
    public class AdminService : IAdminService
    {
        #region Fields
        public readonly IAdminRepository _adminRepository;
        #endregion

        #region Construtor
        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        #endregion

        #region Get
        public async Task<List<UserListModel>> GetUsers()
        {
            return await _adminRepository.GetUserList();
        }

        public async Task<List<DashboardData>> GetDashboardData()
        {
            return await _adminRepository.GetDashboardData();
        }
        #endregion

        #region Post
        public async Task<BaseApiResponse> AddUser(UsersModel users)
        {
            return await _adminRepository.AddUser(users);
        }

        public async Task<BaseApiResponse> DeleteUser(int userId)
        {
            return await _adminRepository.DeleteUser(userId);
        }
        
        public async Task<BaseApiResponse> DeleteMultiUser(SelectedUserModel users)
        {
            return await _adminRepository.DeleteMultiUser(users);
        }
        #endregion
    }
}
