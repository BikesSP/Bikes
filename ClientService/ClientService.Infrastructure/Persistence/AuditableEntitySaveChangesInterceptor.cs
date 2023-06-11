
using ClientService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    //private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<AuditableEntitySaveChangesInterceptor> _logger;

    public AuditableEntitySaveChangesInterceptor(
        //ICurrentUserService currentUserService, 
        ILogger<AuditableEntitySaveChangesInterceptor> logger)
    {
        //_currentUserService = currentUserService;
        _logger = logger;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                //entry.Entity.CreatedBy = _currentUserService.CurrentPrincipal;
                entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                //entry.Entity.UpdatedBy = _currentUserService.CurrentPrincipal;
                entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}

