
using ShopEase.Model.Config;
using Microsoft.Extensions.Options;
using ShopEase.Model.ViewModels.Login;
using System.Data;
using Dapper;
using ShopEase.Common.Helpers;
using System.Reflection;
using ShopEase.Common.EmailNotification;
using ShopEase.Model.Settings;
using static ShopEase.Common.EmailNotification.EmailNotification;
using ShopEase.Model.ViewModels.User;

namespace ShopEase.Data.DBRepository.Account
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        private readonly SMTPSettings _smtpSettings;

        #region Constructor
        public LoginRepository(IOptions<DataConfig> dataConfig, IOptions<SMTPSettings> smtpSettings) : base(dataConfig)
        {
            _smtpSettings = smtpSettings.Value;

        }
        #endregion

        #region Post
        public async Task<LoginResponseModel> LoginUser(LoginRequestModel model)
        {
            var param = new DynamicParameters();
            param.Add("@EmailId", model.EmailId);
            param.Add("@Password", model.Password);
            return await QueryFirstOrDefaultAsync<LoginResponseModel>("LoginUser", param, commandType: CommandType.StoredProcedure);
        }
        #endregion



        #region Post
        public async Task<BaseApiResponse> RegisterUser(RegisterUserRequestModel model)
        {
            BaseApiResponse response= new BaseApiResponse();
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserName", model.UserName);
                param.Add("@UserEmail", model.UserEmail);
                param.Add("@Password", model.Password);
                var result = await QueryFirstOrDefaultAsync<string>("RegisterUser", param, commandType: CommandType.StoredProcedure);

                // Ensure result is not null
                if(result != null && result != "0") 
                {
                    response.Success = true;
                    response.Message = "User registered successfully";

                    
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


                    string subject = "Welcome to ShopEase";
                    // Read the HTML template file
                    string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplate", "CreateNewAdminUser.html");
                    string emailBody = await File.ReadAllTextAsync(templatePath);

                    // Replace placeholders in the template with actual values
                    emailBody = emailBody.Replace("{{UserName}}", model.UserName);
                    //string body = $"Dear {model.UserName},<br/>Welcome to ShopEase! We're glad to have you.";

                    bool emailSent = EmailNotification.SendMailMessage(
                        recipient: model.UserEmail,
                        bcc: null,
                        cc: null,
                        subject: subject,
                        body: emailBody,
                        emailSetting: setting,
                        attachment: null
                    );

                    if (!emailSent)
                    {
                        response.Message = "User registered, but there was an issue sending the welcome email.";
                    }
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while registering the user.";
                
            }
            return response;
        }
        #endregion

        public async Task<UsersModel> GetUserByEmailAsync(string email)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserEmail", email);

                // Call the stored procedure with the dynamic parameters
                var result = await QueryFirstOrDefaultAsync<UsersModel>(
                    "sp_GetUserEmail",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (Exception ex)
            {
                // Handle exception, log if needed
                throw new Exception($"Error fetching user by email: {ex.Message}");
            }
        }


    }
}
