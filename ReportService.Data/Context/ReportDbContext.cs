namespace ReportService.Data.Context
{
    using Microsoft.EntityFrameworkCore;
    using ReportService.Data.Entity;

    public class ReportDbContext : DbContext
    {
        public DbSet<ReportEntity> Reports { get; set; }
        public DbSet<ReportDetailEntity> ReportDetails { get; set; }

        public ReportDbContext(DbContextOptions<ReportDbContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<ReportEntity>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.RequestedAt).IsRequired();
                entity.Property(r => r.Status).IsRequired();
                entity.HasMany(r => r.Details)
                      .WithOne(d => d.Report)
                      .HasForeignKey(d => d.ReportId);
            });

        
            modelBuilder.Entity<ReportDetailEntity>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Location).IsRequired().HasMaxLength(200);
            });

            base.OnModelCreating(modelBuilder);
        }
    }

}
