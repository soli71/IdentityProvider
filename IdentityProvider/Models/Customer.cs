namespace IdentityProvider.Models
{


    public class BaseEntity : BaseEntity<Guid>
    {
    }


    public class CustomerRole : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public Guid RoleId { get; set; }
    }
    public class BaseEntity<T> : ISDeleted, IEntity
    {

        public bool IsDeleted { get; set; }
        public T Id { get; set; }
    }
    public interface IEntity
    { }



    public class User : BaseEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool IsSystemAdmin { get; set; }
    }
}
