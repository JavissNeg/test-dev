using Microsoft.EntityFrameworkCore;
using TestDevBackJR.Domain.Entities;

namespace TestDevBackJR.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Area> Areas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User relationships and cascade behavior
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

        // Username collation (case-sensitive)
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .UseCollation("Latin1_General_CS_AS");

        // Composite index for queries
        modelBuilder.Entity<Login>()
            .HasIndex(l => new { l.UserId, l.Date });
    }
}