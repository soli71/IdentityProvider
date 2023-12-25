namespace IdentityProvider
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public Guid CurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext.GetClaim("nameid");
            if (user == null)
                return new Guid();
            return new Guid(user);
        }

        public List<Guid> GetRoles()
        {
            throw new NotImplementedException();
        }

        public string GetToken()
        {
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            return authHeader;
        }

        public bool IsSystemAdmin() => bool.Parse(_httpContextAccessor.HttpContext.GetClaim("Admin"));
        public string UserName()
        {
            return "";
        }
    }
}
