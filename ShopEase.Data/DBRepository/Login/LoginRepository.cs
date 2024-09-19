
using ShopEase.Model.Config;
using Microsoft.Extensions.Options;
using ShopEase.Model.ViewModels.Login;
using System.Data;
using Dapper;
using ShopEase.Common.Helpers;

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
            return await QueryFirstOrDefaultAsync<LoginResponseModel>(StoredProcedures.LoginUser, param, commandType: CommandType.StoredProcedure);
        }
        #endregion
    }
}
