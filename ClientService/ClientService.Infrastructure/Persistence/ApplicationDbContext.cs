using ClientService.Application.Common.Interfaces;
using ClientService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ClientService.Application.Common.Interfaces;

namespace ClientService.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _saveChangesInterceptor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            AuditableEntitySaveChangesInterceptor saveChangesInterceptor) : base(options)
        {
            _saveChangesInterceptor = saveChangesInterceptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_saveChangesInterceptor);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>();

        }

        public DbSet<Station> Stations => Set<Station>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Trip> Trips => Set<Trip>();
    }
}
