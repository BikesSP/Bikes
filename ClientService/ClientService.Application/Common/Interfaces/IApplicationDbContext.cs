using Microsoft.EntityFrameworkCore;
using ClientService.Domain.Entities;

namespace ClientService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Station> Stations { get; }
        DbSet<Account> Accounts { get; }
        DbSet<Post> Posts { get; }
        DbSet<Trip> Trips { get; }
    }
}
