using Microsoft.EntityFrameworkCore;
using EnsekMeterReadings.Core.Models;

namespace EnsekMeterReadings.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<MeterReadingModel> MeterReadings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountId).IsRequired();
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
            });

            modelBuilder.Entity<MeterReadingModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountId).IsRequired();
                entity.Property(e => e.DateTime).IsRequired();
                entity.Property(e => e.MeterReadValue).IsRequired()
                      .IsRequired()
                      .HasMaxLength(5);
            });
        }
    }
}