using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Controller> Controllers { get; set; }
        public DbSet<RoleAction> RoleActions { get; set; }
        public DbSet<Confirmation> Confirmations { get; set; }
        public DbSet<CustomerRole> CustomerRoles { get; set; }

    }
}
