using ShopEase.Model.ViewModels.Token;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShopEase.Service.Services.JWTAuthentication;
using Microsoft.IdentityModel.Tokens;

namespace ShopEase.Services.JWTAuthentication
{
    public class JWTAuthenticationService : IJWTAuthenticationService
    {
        /// <summary>
        /// Generate JWT token for logging user
        /// </summary>
        /// <param name="userToken">Fields to add in token</param>
        /// <param name="JWT_Secret">Secret Key to generate toekn</param>
        /// <param name="JWT_Validity_Mins">Minutes to expire token</param>
        /// <returns></returns>
        public AccessTokenModel GenerateToken(UserTokenModel userToken, string JWT_Secret, int JWT_Validity_Mins)
        {            
            string serializeToken = JsonConvert.SerializeObject(userToken, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JWT_Secret);

            DateTime tokenGeneratedOnUTC = DateTime.UtcNow;
            int tokenValidityInMins = JWT_Validity_Mins;
            DateTime tokenExpiresOnUTC = tokenGeneratedOnUTC.AddMinutes(tokenValidityInMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, serializeToken)
                }),
                Expires = tokenExpiresOnUTC,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            AccessTokenModel accessTokenVM = new AccessTokenModel();
            accessTokenVM.Token = tokenString;
            accessTokenVM.ValidityInMin = tokenValidityInMins;
            accessTokenVM.ExpiresOnUTC = tokenExpiresOnUTC;
            accessTokenVM.UserId = userToken.UserId;

            return accessTokenVM;
        }

        /// <summary>
        /// Get values from token
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public UserTokenModel GetUserTokenData(string jwtToken)
        {
            UserTokenModel userTokenData = null;
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
            IEnumerable<Claim> claims = securityToken.Claims;

            if (claims != null && claims.ToList().Count > 0)
            {
                var claimData = claims.ToList().FirstOrDefault().Value;
                userTokenData = JsonConvert.DeserializeObject<UserTokenModel>(claimData);
                userTokenData.TokenValidTo = securityToken.ValidTo;
            }
            return userTokenData;
        }
    }
}
