namespace IdentityProvider.Models
{
    public class RoleAction : BaseEntity
    {
        public int ActionId { get; set; }
        public Action Action { get; set; }
        public Guid RoleId { get; set; }
    }
}
