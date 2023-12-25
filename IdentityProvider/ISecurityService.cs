namespace IdentityProvider
{
    public interface ISecurityService
    {
        string GetSha256Hash(string input);
    }
}