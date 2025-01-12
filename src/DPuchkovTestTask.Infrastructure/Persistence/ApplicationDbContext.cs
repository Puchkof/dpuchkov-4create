using DPuchkovTestTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DPuchkovTestTask.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ClinicalTrial> ClinicalTrials { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClinicalTrial>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.TrialId)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.HasIndex(e => e.TrialId)
                .IsUnique();
            
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.DurationInDays)
                .IsRequired();
        });
    }
} 