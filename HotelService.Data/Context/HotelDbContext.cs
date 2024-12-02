using App.Core.BaseClass;
using HotelService.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Data.Context
{
    public class HotelDbContext : DbContext
    {
        public DbSet<HotelEntity> Hotels { get; set; }
        public DbSet<ContactInfoEntity> ContactInfos { get; set; }
        public DbSet<AuthorizedPersonEntity> AuthorizedPersons { get; set; }

        public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HotelEntity>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Name).IsRequired().HasMaxLength(200);
                entity.HasMany(h => h.ContactInfos)
                      .WithOne(c => c.Hotel)
                      .HasForeignKey(c => c.HotelId);
                entity.HasMany(h => h.AuthorizedPersons)
                      .WithOne(a => a.Hotel)
                      .HasForeignKey(a => a.HotelId);
            });

            modelBuilder.Entity<ContactInfoEntity>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Type).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Content).IsRequired().HasMaxLength(500);
            });

            modelBuilder.Entity<AuthorizedPersonEntity>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
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
