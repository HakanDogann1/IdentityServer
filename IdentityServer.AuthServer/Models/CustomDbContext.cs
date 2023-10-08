using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
        {

        }

        public DbSet<CustomUser> CustomUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser()
                {
                    Id = 1,
                    City="Karabük",
                    Email="Admin1@gmail.com",
                    Password="password",
                    UserName="Admin1"
                },
                 new CustomUser()
                 {
                     Id = 2,
                     City = "Ankara",
                     Email = "Admin2@gmail.com",
                     Password = "password",
                     UserName = "Admin2"
                 },
                  new CustomUser()
                  {
                      Id = 3,
                      City = "İstanbul",
                      Email = "Admin3@gmail.com",
                      Password = "password",
                      UserName = "Admin3"
                  }
                );
        }
    }
}
