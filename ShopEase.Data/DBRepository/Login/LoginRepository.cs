
using ShopEase.Model.Config;
using Microsoft.Extensions.Options;
using ShopEase.Model.ViewModels.Login;
using System.Data;
using Dapper;
using ShopEase.Common.Helpers;
using System.Reflection;
using ShopEase.Common.EmailNotification;

namespace ShopEase.Data.DBRepository.Account
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        #region Constructor
        public LoginRepository(IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
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

                    // Send welcome email after successful registration
                    var emailSetting = new EmailNotification.EmailSetting
                    {
                        FromEmail = "jaymin.m@shaligraminfotech.com",   // Replace with your sender email
                        FromName = "ShopEase",                 // Replace with the sender name
                        EmailHostName = "smtp.office365.com",    // Replace with your SMTP server
                        EmailPort = 587,                       // Replace with your SMTP port
                        EmailEnableSsl = true,
                        EmailUsername = "jaymin.m@shaligraminfotech.com",  // Replace with your SMTP username
                        EmailAppPassword = "ndggndqftjdljftb"     // Replace with your SMTP password
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
                        emailSetting: emailSetting,
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
    }
}
