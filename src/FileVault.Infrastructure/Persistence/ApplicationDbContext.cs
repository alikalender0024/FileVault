using FileVault.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileVault.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Node> Nodes => Set<Node>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Configuration
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
        });

        // Node Configuration
        modelBuilder.Entity<Node>(builder =>
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Name).HasMaxLength(255).IsRequired();

            // Self-referencing relationship (Adjacency List)
            builder.HasOne(n => n.Parent)
                   .WithMany(n => n.Children)
                   .HasForeignKey(n => n.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Owner relationship
            builder.HasOne(n => n.Owner)
                   .WithMany(u => u.Nodes)
                   .HasForeignKey(n => n.OwnerId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Performans için indeksler
            builder.HasIndex(n => new { n.OwnerId, n.ParentId });
            builder.HasIndex(n => n.DeletedAt);
        });
    }
}