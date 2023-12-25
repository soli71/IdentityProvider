namespace IdentityProvider.Models
{
    public class Confirmation : BaseEntity<Guid>
    {
        public enum ConfirmatonType
        {
            Mobile = 1,
            ForgotPassword = 2
        }
        public int Code { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpireTime { get; set; }
        public bool Active { get; set; }
        public ConfirmatonType Type { get; set; }
        public string Token { get; set; }
    }
}
