using Charis.Data.Configurations;
using Charis.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Charis.Data.EF
{
    public class CharisDbContext : DbContext
    {
        public CharisDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}