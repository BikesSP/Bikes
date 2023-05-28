using Microsoft.EntityFrameworkCore;
using ClientService.Domain.Entities;

namespace ClientService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Station> Stations { get; }
    }
}
