using Microsoft.AspNetCore.Identity;

namespace IdentityProvider
{
    public static class IdentityExtensions
    {

        public static string JoinErrors(this IEnumerable<string> strs) => string.Join(",", strs);

        public static string JoinIdentityErrors(this IdentityResult identityResult) =>
            identityResult.Errors.Select(x => x.Description).JoinErrors();
        public static string GetToken(this HttpContext context) => context.Request.Headers["Authorization"];

        public static string GetClaim(this HttpContext context, string type) => context?.User?.Claims?.FirstOrDefault(x => x.Type == type)?.Value;
        public static List<string> GetClaims(this HttpContext context, string type) => context.User.Claims?.Where(x => x.Type == type)?.Select(x => x.Value)?.ToList();
    }
}
