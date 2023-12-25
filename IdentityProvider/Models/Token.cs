namespace IdentityProvider.Models
{
    public class Token : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string AccessTokenHash { get; set; }
        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }
    }
}
