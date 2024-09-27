using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopEase.Common.Helpers;
using ShopEase.Model.ViewModels.Login;
using ShopEase.Model.ViewModels.User;
using ShopEase.Service.Services.Account;
using ShopEase.Services.UserProfile;

namespace ShopEase_Api.Controllers.UserProfile
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        #region Fields
        private IUserProfileService _userProfileService;
        #endregion

        #region Constructor
        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        #endregion

        #region Get
        [HttpGet("GetUserProfile")]
        public async Task<ApiPostResponse<UsersModel>> GetUserProfile(int userId)
        {
            ApiPostResponse<UsersModel> response = new ApiPostResponse<UsersModel>();
            UsersModel userResponseModel = new UsersModel();

            UsersModel result = await _userProfileService.GetUserProfileByUserId(userId);

            if (result != null)
            {
                UsersModel model = new UsersModel();
                {
                    model.UserId = result.UserId;
                    model.UserName = result.UserName;
                    model.UserEmail = result.UserEmail;
                    model.Password = result.Password;
                    model.DOB = result.DOB;
                    model.Gender = result.Gender;
                    model.Avatar = result.Avatar;
                };

                userResponseModel = model;
                response.Success = true;
                response.Message = Messages.GetUserProfileSuccess;
            }
            else
            {
                response.Success = false;
                response.Message = Messages.GetUserProfileError;
            }
            response.Data = userResponseModel;
            return response;
        }
        #endregion

        #region Post
        [HttpPost("UpdateUserProfile")]
        public async Task<BaseApiResponse> UpdateUserProfile([FromForm] UsersModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (user.AvatarFile != null && user.AvatarFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine("wwwroot" ,"Documents", "User", "Thumbnail");
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(user.AvatarFile.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        Directory.CreateDirectory(uploadsFolder);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await user.AvatarFile.CopyToAsync(stream);
                        }
                        user.Avatar = fileName;
                    }
                    var result = await _userProfileService.UpdateUserProfile(user);
                    return result;
                }
                else
                {
                    return new ApiResponse<BaseApiResponse>
                    {
                        Success = false,
                        Message = Messages.UpdateUserProfileError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<BaseApiResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        #endregion
    }
}
