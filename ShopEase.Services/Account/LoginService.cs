
using ShopEase.Data.DBRepository.Account;
using ShopEase.Model.ViewModels.Login;

namespace ShopEase.Service.Services.Account
{
    public class LoginService : ILoginService
    {
        #region Fields
        private readonly ILoginRepository _repository;
        #endregion

        #region Construtor
        public LoginService(ILoginRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Post
        public async Task<LoginResponseModel> LoginUser(LoginRequestModel model)
        {
            return await _repository.LoginUser(model);
        } 
        #endregion
    }
}
