using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeBlogAPI.Data
{
    public class AuthDbContext:IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "d7dea1b6-40ed-430e-b28e-9f25b354c789";
            var writerRoleID = "c814992d-c4c8-45e8-a526-33245142ad7d";

            //creating reader and writer roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper(),
                    ConcurrencyStamp= readerRoleId
                },
                new IdentityRole
                {
                    Id = writerRoleID,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper(),
                    ConcurrencyStamp= writerRoleID
                }

            };
            //seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Create Admin User
            var adminID = "49aa06f4-10ed-4309-b4a1-4fb2f8511cf3";
            var admin = new IdentityUser
            {
                UserName = "admin@codeblog.com",
                Email = "admin@codeblog.com",
                Id = adminID,
                NormalizedEmail = "admin@codeblog.com".ToUpper(),
                NormalizedUserName = "admin@codeblog.com".ToUpper(),
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@05");
            builder.Entity<IdentityUser>().HasData(admin);

            //give roles to admin
            var adminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = readerRoleId,
                    UserId = adminID,
                },
                new IdentityUserRole<string>
                {
                    RoleId = writerRoleID,
                    UserId = adminID,
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
