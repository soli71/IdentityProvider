using IdentityProvider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityProvider;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class Authentication
{
    private readonly RequestDelegate _next;

    public Authentication(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        var endpoint = httpContext.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() == null)
        {
            try
            {
                var token = httpContext.GetToken();

                var tokenHandler = new JwtSecurityTokenHandler();
                var secretkey = Encoding.UTF8.GetBytes("M1CF4B7DC4C4175B7788BE4F55GA6");
                var encryptionkey = Encoding.UTF8.GetBytes("J+7l^@tgvarlxb0n");
                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretkey),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero,
                        TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var claimIdentity = jwtToken.Claims;

                    httpContext.User.AddIdentity(new ClaimsIdentity(claimIdentity));
                }
                catch
                {
                    // return null if validation fails
                    throw new UnAuthorizeException();
                }
            }
            catch
            {
                throw new UnAuthorizeException();
            }
        }

        return _next(httpContext);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class AuthenticationExtensions
{
    public static IApplicationBuilder UseAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<Authentication>();
    }
}

public class ApplicationDbInitializer
{
    public static void SeedAdminUser(AppDbContext dbContext)
    {
        var existAdminUser = dbContext.Users.Any(x => x.IsSystemAdmin);
        if (!existAdminUser)
        {
            var customer = new User
            {
                Mobile = "09121111111",
                IsSystemAdmin = true
            };
            dbContext.Users.Add(customer);
            dbContext.SaveChanges();
        }
    }
}
