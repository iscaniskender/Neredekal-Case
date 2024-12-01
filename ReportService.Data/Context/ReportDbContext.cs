using App.Core.BaseClass;
using ReportService.Data.Entity;

namespace ReportService.Data.Context
{
    using Microsoft.EntityFrameworkCore;

    public class ReportDbContext : DbContext
    {
        public DbSet<ReportEntity> Reports { get; set; }

        public ReportDbContext(DbContextOptions<ReportDbContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportEntity>(entity =>
            {
                entity.HasKey(r => r.Id);
            });
            
            base.OnModelCreating(modelBuilder);
        }
        
        public override int SaveChanges()
        {
            var data = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in data)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.IsActive=true;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.Now;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.Entity.UpdatedAt = DateTime.Now;
                    entry.Entity.IsActive = false;
                    entry.State= EntityState.Modified;
                }
            }
            return base.SaveChanges();
        }
    }

}
