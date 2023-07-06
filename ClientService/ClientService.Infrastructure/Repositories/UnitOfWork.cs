
using ClientService.Domain.Entities;
using ClientService.Infrastructure.Persistence;
using ClientService.Infrastructure.Repositories.AccountRepository;
using ClientService.Infrastructure.Repositories.ExponentPushTokenRepository;
using ClientService.Infrastructure.Repositories.PostRepository;
using ClientService.Infrastructure.Repositories.StationRepository;
using ClientService.Infrastructure.Repositories.TripRepository;
using Google;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{

    private readonly ApplicationDbContext _dbContext;
    private bool _disposed;

    private IAccountRepository _accountRepository;
    private IStationRepository _stationRepository;
    private ITripRepository _tripRepository;
    private IPostRepository _postRepository;
    private IExponentPushTokenRepostiory _exponentPushTokenRepostiory;

    public IAccountRepository AccountRepository
    {
        get => _accountRepository ??= new AccountRepository.AccountRepository(_dbContext);
    }

    public IStationRepository StationRepository
    {
        get => _stationRepository ??= new StationRepository.StationRepository(_dbContext);
    }

    public ITripRepository TripRepository
    {
        get => _tripRepository ??= new TripRepository.TripRepository(_dbContext);
    }

    public IPostRepository PostRepository
    {
        get => _postRepository ??= new PostRepository.PostRepository(_dbContext);
    }
    public IExponentPushTokenRepostiory ExponentPushTokenRepostiory
    {
        get => _exponentPushTokenRepostiory ??= new ExponentPushTokenRepostiory(_dbContext);
    }


    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Rollback()
    {
        foreach (var entry in _dbContext.ChangeTracker.Entries())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _dbContext.Dispose();

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
