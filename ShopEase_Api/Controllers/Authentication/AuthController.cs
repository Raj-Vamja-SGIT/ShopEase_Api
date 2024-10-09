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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using ShopEase.Model.ViewModels.User;
using System.Reflection;
using Google.Apis.Auth;
using ShopEase.Data.DBRepository.Account;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ShopEase_Api.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private ILoginService _accountService;
        private readonly AppSettings _appSettings;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly IConfiguration _config;
        private readonly ILoginRepository _loginRepo;
        #endregion

        #region Constructor
        public AuthController(ILoggerManager logger,
                               ILoginService accountService,
                               IOptions<AppSettings> appSettings,
                               IJWTAuthenticationService jwtAuthenticationService,
                               ILoginRepository loginRepo)
        {
            _logger = logger;
            _accountService = accountService;
            _jwtAuthenticationService = jwtAuthenticationService;
            _appSettings = appSettings.Value;
            _loginRepo = loginRepo;
        }
        #endregion

        #region Post
        /// <summary>
        /// User will login by passing emailid and password
        /// </summary>
        /// <param name="model">object with emailid and password details in request</param>
        /// <returns></returns>
        [HttpPost("Login")]
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
                UserProfile.UserName = result.UserName;
                UserProfile.EmailId = result.UserEmail;
                UserProfile.RoleId = result.RoleId;

                loginBaseReponseModel.AccessToken = AccessToken;
                loginBaseReponseModel.UserProfile = UserProfile;

                response.Success = true;
                response.Message = Messages.LoginSuccess;
            }
            else
            {
                response.Success = false;
                response.Message = Messages.LoginError;
            }
            response.Data = loginBaseReponseModel;
            return response;

        }


        [HttpPost("LoginWithGoogle")]
        public async Task<ApiPostResponse<LoginWithGoogleBaseReponseModel>> LoginWithGoogle([FromBody] string credential)
        {
            var setting = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _appSettings.GoogleClientId },
                IssuedAtClockTolerance = TimeSpan.FromMinutes(5),
            };

            var response = new ApiPostResponse<LoginWithGoogleBaseReponseModel>();
            var payLoad = await GoogleJsonWebSignature.ValidateAsync(credential, setting);

            if (payLoad == null || string.IsNullOrWhiteSpace(payLoad.Email))
            {
                return new ApiPostResponse<LoginWithGoogleBaseReponseModel>
                {
                    Success = false,
                    Message = Messages.LoginIssue
                };
            }

            var userDetails = await _loginRepo.GetUserByEmailAsync(payLoad.Email);
            if (userDetails == null)
            {
                // Register the user if not found
                var registerUserRequestModel = new RegisterUserRequestModel
                {
                    UserName = payLoad.Name,
                    UserEmail = payLoad.Email,
                    Password = "123456"
                };

                RegisterUser(registerUserRequestModel);
                userDetails = await _loginRepo.GetUserByEmailAsync(payLoad.Email);
            }

            if (userDetails != null)
            {
                var objTokenData = new UserTokenModel
                {
                    EmailId = userDetails.UserEmail,
                    UserId = userDetails.UserId,
                    UserName = userDetails.UserName
                };

                var objAccessTokenData = _jwtAuthenticationService.GenerateToken(objTokenData, _appSettings.JWT_Secret, _appSettings.JWT_Validity_Mins);

                var accessToken = new AccessTokenResponseModel
                {
                    Token = objAccessTokenData.Token,
                    ExpiresOnUTC = objAccessTokenData.ExpiresOnUTC
                };

                var loginWithGoogleBaseResponseModel = new LoginWithGoogleBaseReponseModel
                {
                    UserProfile = userDetails,
                    AccessToken = accessToken
                };

                response.Success = true;
                response.Message = Messages.LoginSuccess;
                response.Data = loginWithGoogleBaseResponseModel;

                return response;
            }

            return new ApiPostResponse<LoginWithGoogleBaseReponseModel>
            {
                Success = false,
                Message = Messages.LoginIssue
            };
        }


        //public async Task<BaseApiResponse> LoginWithGoogle([FromBody] string credential)
        //{
        //    var setting = new GoogleJsonWebSignature.ValidationSettings()
        //    {
        //        Audience = new List<string> { _appSettings.GoogleClientId },
        //        IssuedAtClockTolerance = TimeSpan.FromMinutes(5),
        //    };

        //    ApiPostResponse<LoginWithGoogleBaseReponseModel> response = new ApiPostResponse<LoginWithGoogleBaseReponseModel>();
        //    LoginWithGoogleBaseReponseModel loginWithGooglrBaseReponseModel = new LoginWithGoogleBaseReponseModel();

        //    var payLoad = await GoogleJsonWebSignature.ValidateAsync(credential, setting);
        //    if (payLoad != null && !string.IsNullOrWhiteSpace(payLoad.Email))
        //    {
        //        var userDetails = await _loginRepo.GetUserByEmailAsync(payLoad.Email);
        //        if (userDetails != null)
        //        {
        //            UserTokenModel objTokenData = new UserTokenModel();
        //            objTokenData.EmailId = userDetails.UserEmail;
        //            objTokenData.UserId = userDetails.UserId;
        //            objTokenData.UserName = userDetails.UserName;

        //            AccessTokenModel objAccessTokenData = _jwtAuthenticationService.GenerateToken(objTokenData, _appSettings.JWT_Secret, _appSettings.JWT_Validity_Mins);

        //            AccessTokenResponseModel AccessToken = new AccessTokenResponseModel();
        //            AccessToken.Token = objAccessTokenData.Token;
        //            AccessToken.ExpiresOnUTC = objAccessTokenData.ExpiresOnUTC;

        //            loginWithGooglrBaseReponseModel.UserProfile = userDetails;
        //            loginWithGooglrBaseReponseModel.AccessToken = AccessToken;

        //            response.Success = true;
        //            response.Message = Messages.LoginSuccess;
        //            response.Data = loginWithGooglrBaseReponseModel;

        //            return response;
        //        }
        //        else
        //        {
        //            RegisterUserRequestModel registerUserRequestModel = new RegisterUserRequestModel();
        //            {
        //                registerUserRequestModel.UserName = payLoad.Name;
        //                registerUserRequestModel.UserEmail = payLoad.Email;
        //                registerUserRequestModel.Password = "123456";
        //            }
        //            RegisterUser(registerUserRequestModel);

        //            var registeredUserDetails = await _loginRepo.GetUserByEmailAsync(payLoad.Email);
        //            if (registeredUserDetails != null)
        //            {
        //                UserTokenModel objTokenData = new UserTokenModel();
        //                objTokenData.EmailId = registeredUserDetails.UserEmail;
        //                objTokenData.UserId = registeredUserDetails.UserId;
        //                objTokenData.UserName = registeredUserDetails.UserName;

        //                AccessTokenModel objAccessTokenData = _jwtAuthenticationService.GenerateToken(objTokenData, _appSettings.JWT_Secret, _appSettings.JWT_Validity_Mins);

        //                AccessTokenResponseModel AccessToken = new AccessTokenResponseModel();
        //                AccessToken.Token = objAccessTokenData.Token;
        //                AccessToken.ExpiresOnUTC = objAccessTokenData.ExpiresOnUTC;

        //                loginWithGooglrBaseReponseModel.UserProfile = registeredUserDetails;
        //                loginWithGooglrBaseReponseModel.AccessToken = AccessToken;

        //                response.Success = true;
        //                response.Message = Messages.LoginSuccess;
        //                response.Data = loginWithGooglrBaseReponseModel;

        //                return response;
        //            }
        //            else
        //            {
        //                return new ApiPostResponse<BaseApiResponse>
        //                {
        //                    Success = false,
        //                    Message = Messages.LoginIssue
        //                };
        //            }
        //        }
        //    }
        //    return new ApiPostResponse<BaseApiResponse>
        //    {
        //        Success = false,
        //        Message = Messages.LoginIssue
        //    };
        //}

        [HttpPost("Register")]
        public async Task<BaseApiResponse> RegisterUser([FromBody] RegisterUserRequestModel model)
        {
            try
            {
                var chekUser = await _loginRepo.GetUserByEmailAsync(model.UserEmail);
                if (chekUser != null)
                {
                    return new ApiPostResponse<BaseApiResponse>
                    {
                        Success = false,
                        Message = Messages.UserExist,
                    };
                }

                if (!ModelState.IsValid)
                {
                    return new ApiPostResponse<BaseApiResponse>
                    {
                        Success = false,
                        Message = "Invalid model state",
                    };
                }

                var result = await _accountService.RegisterUser(model);
                return result;

            }
            catch (Exception ex)
            {
                // Log exception details here if necessary
                return new ApiPostResponse<BaseApiResponse>
                {
                    Success = false,
                    Message = "An error occurred while processing your request.",
                };
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestModel model)
        {
            var result = await _accountService.ForgotPassword(model);
            return Ok(result);
        }

        [HttpPost("ChangeUserPassword")]
        public async Task<BaseApiResponse> ChangeUserPassword([FromBody] UsersModel users)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ApiPostResponse<BaseApiResponse>
                    {
                        Success = false,
                        Message = "Invalid model state",
                    };
                }
                var result = await _accountService.ChangeUserPassword(users);
                return result;
            }
            catch (Exception ex)
            {
                return new ApiPostResponse<BaseApiResponse>
                {
                    Success = false,
                    Message = "An error occurred while processing your request.",
                };
            }
        }
    }
    #endregion

}
