using ClientService.Domain.Entities;
using System.Security.Principal;

namespace ClientService.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IBaseRepository<Station> StationRepository { get; }
    IBaseRepository<Account> AccountRepository { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync();

    void Rollback();
}
