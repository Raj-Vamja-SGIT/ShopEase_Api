using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShopEase.Logger;
using ShopEase.Common.Helpers;
using ShopEase.Model.ViewModels.Login;
using ShopEase.Model.Settings;
using ShopEase.Model.ViewModels.Token;
using ShopEase.Service.Services.JWTAuthentication;
using ShopEase.Service.Services.Account;

namespace ShopEase_Api.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private ILoginService _accountService;
        private readonly AppSettings _appSettings;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        #endregion

        #region Constructor
        public LoginController(ILoggerManager logger,
                               ILoginService accountService,
                               IOptions<AppSettings> appSettings,
                               IJWTAuthenticationService jwtAuthenticationService)
        {
            _logger = logger;
            _accountService = accountService;
            _jwtAuthenticationService = jwtAuthenticationService;
            _appSettings = appSettings.Value;
        }
        #endregion

        #region Post
        /// <summary>
        /// User will login by passing emailid and password
        /// </summary>
        /// <param name="model">object with emailid and password details in request</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiPostResponse<LoginBaseReponseModel>> LoginUser([FromBody] LoginRequestModel model)
        {
            ApiPostResponse<LoginBaseReponseModel> response = new ApiPostResponse<LoginBaseReponseModel>();
            LoginBaseReponseModel loginBaseReponseModel = new LoginBaseReponseModel();

            //model.Password = EncryptionDecryption.GetEncrypt(model.Password);
            LoginResponseModel result = await _accountService.LoginUser(model);

            if (result != null && result.UserId > 0)
            {
                UserTokenModel objTokenData = new UserTokenModel();
                objTokenData.EmailId = model.EmailId;
                objTokenData.UserId = result.UserId;
                objTokenData.UserName = result.UserName;    

                AccessTokenModel objAccessTokenData = _jwtAuthenticationService.GenerateToken(objTokenData, _appSettings.JWT_Secret, _appSettings.JWT_Validity_Mins);

                AccessTokenResponseModel AccessToken = new AccessTokenResponseModel();
                AccessToken.Token = objAccessTokenData.Token;
                AccessToken.ExpiresOnUTC = objAccessTokenData.ExpiresOnUTC;

                UserProfileModel UserProfile = new UserProfileModel();
                UserProfile.UserId = result.UserId;
                UserProfile.EmailId = result.EmailId;

                loginBaseReponseModel.AccessToken = AccessToken;
                loginBaseReponseModel.UserProfile = UserProfile;

                response.Success = true;
                response.Message = ErrorMessages.LoginSuccess;
            }
            else
            {
                response.Success = false;
                response.Message = ErrorMessages.LoginError;
            }
            response.Data = loginBaseReponseModel;
            return response;

        }

        #endregion

    }
}
