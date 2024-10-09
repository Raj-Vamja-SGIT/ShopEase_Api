using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopEase.Common.Helpers;
using ShopEase.Data;
using ShopEase.Data.DBRepository.Account;
using ShopEase.Model.ViewModels.User;
using ShopEase.Services.Admin;
using ShopEase.Services.UserProfile;

namespace ShopEase_Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        #region Fields
        private IAdminService _adminService;
        private ILoginRepository _loginRepository;
        #endregion

        #region Constructor
        public AdminController(IAdminService adminService, ILoginRepository loginRepository)
        {
            _adminService = adminService;
            _loginRepository = loginRepository;
        }
        #endregion

        #region Get
        [HttpGet("GetUsers")]
        public async Task<ApiResponse<UserListModel>> GetUsers()
        {
            ApiResponse<UserListModel> response = new ApiResponse<UserListModel> { Data = new List<UserListModel>() };
            try
            {
                var results = await _adminService.GetUsers();
                if (results != null)
                {
                    response.Data = results;
                    response.Success = true;
                    response.Message = Messages.GetUsersSuccess;
                }
            }
            catch (Exception ex)
            {
                response = new ApiResponse<UserListModel>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
            return response;
        }

        [HttpGet("GetDashBoardData")]
        public async Task<ApiResponse<DashboardData>> GetDashBoardData()
        {
            ApiResponse<DashboardData> response = new ApiResponse<DashboardData> { Data = new List<DashboardData>() };
            try
            {
                var results = await _adminService.GetDashboardData();
                if (results != null)
                {
                    response.Data = results;
                    response.Success = true;
                    response.Message = Messages.DashbodrdDataSuccess;
                }
            }
            catch (Exception ex)
            {
                response = new ApiResponse<DashboardData>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
            return response;
        }

        #endregion

        #region Post
        [HttpPost("AddUser")]
        public async Task<BaseApiResponse> AddUser([FromForm] UsersModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (user.Avatar != null && user.AvatarFile == null)
                    {
                        user.Avatar = user.Avatar;
                    }
                    if ((user.AvatarFile != null || (user.Avatar != null && user.AvatarFile != null)) && user.AvatarFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine("wwwroot", "Documents", "User", "Thumbnail");
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(user.AvatarFile.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        Directory.CreateDirectory(uploadsFolder);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await user.AvatarFile.CopyToAsync(stream);
                        }
                        user.Avatar = fileName;
                    }
                    var result = await _adminService.AddUser(user);
                    return result;
                }
                else
                {
                    return new ApiPostResponse<BaseApiResponse>
                    {
                        Success = false,
                        Message = Messages.AddUserError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiPostResponse<BaseApiResponse>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPost("DeleteUser")]
        public async Task<BaseApiResponse> DeleteUser(int userId)
        {
            try
            {
                if (userId != null && userId > 0)
                {
                    var result = await _adminService.DeleteUser(userId);
                    return result;
                }
                else
                {
                    return new ApiPostResponse<BaseApiResponse>
                    {
                        Success = false,
                        Message = Messages.UserNotFound
                    };
                }
            }
            catch (Exception ex)
            {

                return new ApiPostResponse<BaseApiResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPost("DeleteMultiUser")]
        public async Task<BaseApiResponse> DeleteMultiUser(SelectedUserModel users)
        {
            try
            {
                if (users.userIds != null)
                {
                    var result = await _adminService.DeleteMultiUser(users);
                    return result;
                }
                else
                {
                    return new ApiPostResponse<BaseApiResponse>
                    {
                        Success = false,
                        Message = Messages.UserNotFound
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiPostResponse<BaseApiResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        #endregion
    }
}
