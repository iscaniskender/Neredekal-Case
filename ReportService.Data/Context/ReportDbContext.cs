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
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
    
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.IsActive = true;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.IsActive = false;
                        entry.State = EntityState.Modified;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }

}
