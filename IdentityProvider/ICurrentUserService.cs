namespace IdentityProvider
{
    public interface ICurrentUserService
    {
        Guid CurrentUserId();
        string UserName();
        string GetToken();
        bool IsSystemAdmin();
        List<Guid> GetRoles();
    }
}
