using IdentityProvider.Dtos;
using IdentityProvider.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityProvider
{
    public class TokenService : ITokenService
    {
        private readonly ISecurityService _securityService;
        private readonly AppDbContext _dbContext;
        public TokenService(
            ISecurityService securityService, AppDbContext dbContext)
        {

            _securityService = securityService;
            _dbContext = dbContext;
        }
        public ApiResult<string> GenerateToken(User tokenDto)
        {


            var secretkey = Encoding.UTF8.GetBytes("M1CF4B7DC4C4175B7788BE4F55GA6");
            var signInCredentials = new SigningCredentials(new SymmetricSecurityKey(secretkey), SecurityAlgorithms.HmacSha256);

            var encryptionKey = Encoding.UTF8.GetBytes("J+7l^@tgvarlxb0n");
            var encryptingCredentials =
                new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);


            IEnumerable<Claim> claims = SetClaims(tokenDto);



            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "",
                Audience = "",
                Expires = DateTime.UtcNow.AddMinutes(500000),
                EncryptingCredentials = encryptingCredentials,
                SigningCredentials = signInCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            string encryptedJwt = tokenHandler.WriteToken(securityToken);

            return ApiResultHandler<string>.Ok(encryptedJwt);
        }

        private IEnumerable<Claim> SetClaims(User customer)
        {
            var customerRoles = _dbContext.CustomerRoles.Where(x => x.CustomerId == customer.Id);
            var roleClaims = new List<Claim>();
            SetRolesClaim(customerRoles.Select(x => x.RoleId.ToString()).ToList(), roleClaims);

            IEnumerable<Claim> claims = SetAllClaims(customer, roleClaims);
            return claims;
        }
        private static void SetRolesClaim(IList<string> roles, List<Claim> roleClaims)
        {
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
        }
        private static IEnumerable<Claim> SetAllClaims(User customer, List<Claim> roleClaims)
        {
            return new[]
            {
                new Claim(ClaimTypes.Name, customer.Mobile),
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                new Claim("Admin",customer.IsSystemAdmin.ToString())
            }
            .Union(roleClaims);
        }


        public ApiResult DeleteToken(string token)
        {
            var customerToken = _dbContext.Tokens.FirstOrDefault(x => x.AccessTokenHash == _securityService.GetSha256Hash(token));
            if (customerToken is null)
            {
                return ApiResultHandler.Failed("Token Invalid!!", code: System.Net.HttpStatusCode.Unauthorized);
            }
            _dbContext.Tokens.Remove(customerToken);
            _dbContext.SaveChanges();
            return ApiResultHandler.Ok();
        }


        public void AddUserToken(Token userToken)
        {
            userToken.AccessTokenHash = _securityService.GetSha256Hash(userToken.AccessTokenHash);
            _dbContext.Tokens.Add(userToken);
            _dbContext.SaveChanges();
        }




        public bool ExistToken(string token)
        {
            var tokenHash = _securityService.GetSha256Hash(token);
            return _dbContext.Tokens.Any(x => x.AccessTokenHash == tokenHash);
        }


        //public bool UserSecurityStampValid(Guid userId, string securityStamp) =>
        //    _userManager.Users.Any(x => x.Id == userId && x.SecurityStamp == securityStamp);

    }
}
