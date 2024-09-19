using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;
using ShopEase.Logger;
using ShopEase.Common.CommonMethods;
using ShopEase.Model.ViewModels.Common;
using ShopEase.Common.Helpers;

using ShopEase.Model.ViewModels.Token;
using ShopEase.Service.Services.JWTAuthentication;
using System.Text;

namespace ShopEase_Api.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="memoryCache"></param>
        public CustomMiddleware(RequestDelegate next,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IJWTAuthenticationService jwtAuthenticationService, IHttpContextAccessor httpContextAccessor, ILoggerManager logger)
        {
            _next = next;
            _hostingEnvironment = hostingEnvironment;
            _jwtAuthenticationService = jwtAuthenticationService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Invoke on every request response
        /// </summary>
        /// <param name="context"></param>
        /// <param name="settingService"></param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Delete files from folder for logs of request and response older than 7 days.
                DeleteOldReqResLogFiles();

                // Check JWT Token validity expiry
                string jwtToken = context.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme, " ");
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    UserTokenModel userTokenModel = _jwtAuthenticationService.GetUserTokenData(jwtToken);

                    if (userTokenModel != null)
                    {
                        if (userTokenModel.TokenValidTo < DateTime.UtcNow.AddMinutes(1))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                // Add error logs in folder
                AddExceptionLogsToLoggerFile(context, ex);
            }
        }

        /// <summary>
        /// Delete files from error logs folder which is older than 7 days.
        /// </summary>
        public void DeleteOldReqResLogFiles()
        {
            var directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ReqResLog");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);

                int days = -7;
                if (_hostingEnvironment.IsProduction())
                {
                    days = -30;
                }

                if (fi.LastAccessTime < DateTime.Now.AddDays(days))
                {
                    fi.Delete();
                }
            }
        }

        /// <summary>
        /// Store exception in logger file
        /// </summary>
        private void AddExceptionLogsToLoggerFile(HttpContext context, Exception exception)
        {
            ParamValue paramValues = CommonMethod.GetKeyValues(context);
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine + ShopEase.Common.Helpers.Constants.RequestParams + paramValues.HeaderValue +
                      Environment.NewLine + ShopEase.Common.Helpers.Constants.QueryStringParams + paramValues.QueryStringValue +
                      Environment.NewLine + ShopEase.Common.Helpers.Constants.RequestMessage + exception.Message);
            _logger.Error(sb.ToString());
        }
    }
}
