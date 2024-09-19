
using ShopEase.Model.Config;
using Microsoft.Extensions.Options;
using ShopEase.Model.ViewModels.Login;
using System.Data;
using Dapper;
using ShopEase.Common.Helpers;
using System.Reflection;

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
