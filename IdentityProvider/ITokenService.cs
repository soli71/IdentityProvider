using IdentityProvider.Dtos;
using IdentityProvider.Models;

namespace IdentityProvider
{
    public interface ITokenService
    {
        void AddUserToken(Token userToken);
        ApiResult DeleteToken(string token);
        bool ExistToken(string token);
        ApiResult<string> GenerateToken(User tokenDto);
    }
}