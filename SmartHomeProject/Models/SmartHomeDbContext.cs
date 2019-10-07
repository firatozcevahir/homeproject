using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SmartHomeProject.Models
{
    public partial class SmartHomeDbContext : DbContext
    {
        public SmartHomeDbContext()
        {
        }
        const string ConStr = "Data Source=.;Initial Catalog=SmartHomeDb;Persist Security Info=True;Integrated Security = true";
        public SmartHomeDbContext(DbContextOptions<SmartHomeDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Light> Light { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Light>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Description).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
