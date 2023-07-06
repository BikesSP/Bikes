using ClientService.Domain.Entities;
using ClientService.Infrastructure.Repositories.AccountRepository;
using ClientService.Infrastructure.Repositories.ExponentPushTokenRepository;
using ClientService.Infrastructure.Repositories.PostRepository;
using ClientService.Infrastructure.Repositories.StationRepository;
using ClientService.Infrastructure.Repositories.TripRepository;
using System.Security.Principal;

namespace ClientService.Infrastructure.Repositories;

public interface IUnitOfWork
{
    public IStationRepository StationRepository { get; }
    public IAccountRepository AccountRepository { get; }
    public ITripRepository TripRepository { get; }
    public IPostRepository PostRepository { get; }
    public IExponentPushTokenRepostiory ExponentPushTokenRepostiory { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync();

    void Rollback();
}
