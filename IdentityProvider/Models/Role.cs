namespace IdentityProvider.Models
{
    public class Role : BaseEntity
    {

        public string Name { get; set; }
        public Role()
        {
            Id = Guid.NewGuid();
        }
        public Role(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
