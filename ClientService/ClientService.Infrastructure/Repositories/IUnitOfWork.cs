using ClientService.Domain.Entities;
using ClientService.Infrastructure.Repositories.AccountRepository;
using ClientService.Infrastructure.Repositories.StationRepository;
using System.Security.Principal;

namespace ClientService.Infrastructure.Repositories;

public interface IUnitOfWork
{
    public IStationRepository StationRepository { get; }
    public IAccountRepository AccountRepository { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync();

    void Rollback();
}
