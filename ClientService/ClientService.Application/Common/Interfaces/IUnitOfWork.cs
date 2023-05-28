using ClientService.Domain.Entities;
using System.Security.Principal;

namespace ClientService.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IBaseRepository<Station> StationRepository { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync();

    void Rollback();
}
