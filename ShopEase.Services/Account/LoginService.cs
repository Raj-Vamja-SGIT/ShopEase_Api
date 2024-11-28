
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ShopEase.Common.Helpers;
using ShopEase.Data.DBRepository.Account;
using ShopEase.Model.Settings;
using ShopEase.Model.ViewModels.Login;
using ShopEase.Model.ViewModels.User;
using static ShopEase.Common.EmailNotification.EmailNotification;
using System.Security.Cryptography;
using ShopEase.Common.Helpers;

namespace ShopEase.Service.Services.Account
{
    public class LoginService : ILoginService
    {
        #region Fields
        private readonly ILoginRepository _repository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SMTPSettings _smtpSettings;

        #endregion

        #region Construtor
        public LoginService(ILoginRepository repository, IOptions<SMTPSettings> smtpSettings)
        {
            _repository = repository;
            _smtpSettings = smtpSettings.Value;

        }
        #endregion

        #region Post
        public async Task<LoginResponseModel> LoginUser(LoginRequestModel model)
        {
            return await _repository.LoginUser(model);
        }

        public async Task<BaseApiResponse> RegisterUser(RegisterUserRequestModel model)
        {
            return await _repository.RegisterUser(model);
        }
        private async void SendEmailOnForgotPassword(string email, string url)
        {
            EmailSetting setting = new()
            {
                EmailEnableSsl = Convert.ToBoolean(_smtpSettings.EmailEnableSsl),
                EmailHostName = _smtpSettings.EmailHostName,
                EmailAppPassword = _smtpSettings.EmailAppPassword,
                EmailPort = Convert.ToInt32(_smtpSettings.EmailPort),
                FromEmail = _smtpSettings.FromEmail,
                FromName = _smtpSettings.FromName,
                EmailUsername = _smtpSettings.EmailUsername
            };

            string emailBody = "";

            // This is the directory where the email templates are stored
            string templateDirectory = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplate");

            // Ensure the directory exists
            if (!Directory.Exists(templateDirectory))
            {
                Directory.CreateDirectory(templateDirectory);
            }

            // This is the full path to the ForgotPassword.html file
            string filePath = Path.Combine(templateDirectory, "ForgotPassword.html");

            // Ensure the file exists before attempting to read
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    emailBody = reader.ReadToEnd();
                }

                emailBody = emailBody.Replace("##EmailAddress##", email);
                emailBody = emailBody.Replace("##Url##", url);

                await SendAsyncEmail(email, "", "", "Reset Password", emailBody, setting, "");
            }
            else
            {
                throw new FileNotFoundException($"The email template file '{filePath}' was not found.");
            }
        }

        public async Task<BaseApiResponse> ForgotPassword(ForgotPasswordRequestModel model)
        {
            var user = await _repository.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                //var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                string token = GenerateSecureToken();
                var timeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                var timeStampedToken = $"{token}~{timeStamp}";
                var callback = (model.ClientURL + "?token=" + timeStampedToken + "&email=" + model.Email);
                SendEmailOnForgotPassword(model.Email, callback);

                return new BaseApiResponse { Success = true, Message = Messages.ResetPasswordMailSuccess };
            }
            else
            {
                return new BaseApiResponse { Success = false, Message = Messages.UserNotFound };
            }
        }

        private string GenerateSecureToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32]; // 256 bits
                rng.GetBytes(tokenData);

                // Convert token to a URL-safe Base64 string
                return Convert.ToBase64String(tokenData)
                    .Replace("+", "-")
                    .Replace("/", "_")
                    .TrimEnd('=');
            }
        }

        public async Task<BaseApiResponse> ChangeUserPassword(UsersModel user)
        {
            return await _repository.ChangeUserPassword(user);
        }

        #endregion
    }
}
