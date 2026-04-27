using Microsoft.EntityFrameworkCore;
using TestDevBackJR.Domain.Entities;

namespace TestDevBackJR.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<UserStatus> UserStatuses { get; set; }
    public DbSet<UserType> UserTypes { get; set; }
    public DbSet<AreaStatus> AreaStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserStatus>()
            .HasKey(us => us.Id);
        modelBuilder.Entity<UserStatus>()
            .HasMany(us => us.Users)
            .WithOne(u => u.UserStatus)
            .HasForeignKey(u => u.UserStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserType>()
            .HasKey(ut => ut.Id);
        modelBuilder.Entity<UserType>()
            .HasMany(ut => ut.Users)
            .WithOne(u => u.UserType)
            .HasForeignKey(u => u.UserTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AreaStatus>()
            .HasKey(ast => ast.Id);
        modelBuilder.Entity<AreaStatus>()
            .HasMany(ast => ast.Areas)
            .WithOne(a => a.AreaStatus)
            .HasForeignKey(a => a.AreaStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Logins)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Area)
            .WithMany(a => a.Users)
            .HasForeignKey(u => u.AreaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .UseCollation("Latin1_General_CS_AS");

        modelBuilder.Entity<Login>()
            .HasIndex(l => new { l.UserId, l.Date, l.Id })
            .IncludeProperties(l => new { l.MovementType });
    }
}