using AuthService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles {  get; set; }
        public DbSet<ApplicationUserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionRole> PermissionRoles { get; set; }

        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PermissionRole>()
                .HasKey(pr => new { pr.PermissionId, pr.RoleId });

            builder.Entity<PermissionRole>()
                .HasOne(pr => pr.Permission)
                .WithMany(p => p.PermissionRoles)
                .HasForeignKey(pr => pr.PermissionId);

            builder.Entity<PermissionRole>()
                .HasOne(p => p.Role)
                .WithMany(p => p.PermissionRoles)
                .HasForeignKey(pr => pr.RoleId);

            builder.Entity<Permission>()
                .HasIndex(p => p.Code)
                .IsUnique();
        }
    }
}
