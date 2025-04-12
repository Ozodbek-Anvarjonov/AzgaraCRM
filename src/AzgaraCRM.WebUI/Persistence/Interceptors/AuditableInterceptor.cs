using AzgaraCRM.WebUI.Domain.Common;
using AzgaraCRM.WebUI.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AzgaraCRM.WebUI.Persistence.Interceptors;

public class AuditableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var auditableEntry = eventData.Context!.ChangeTracker.Entries<AuditableEntity>();
        var userId = HttpContextHelper.UserId ?? HttpContextHelper.SystemId;
        var now = DateTime.UtcNow;

        foreach (var entry in auditableEntry)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(nameof(AuditableEntity.CreatedAt)).CurrentValue = now;
                    entry.Property(nameof(AuditableEntity.CreatedBy)).CurrentValue = userId;
                    break;

                case EntityState.Modified:
                    entry.Property(nameof(AuditableEntity.ModifiedAt)).CurrentValue = now;
                    entry.Property(nameof(AuditableEntity.ModifiedBy)).CurrentValue = userId;
                    break;

                case EntityState.Deleted:
                    entry.Property(nameof(AuditableEntity.DeletedAt)).CurrentValue = now;
                    entry.Property(nameof(AuditableEntity.DeletedBy)).CurrentValue = userId;
                    entry.Property(nameof(AuditableEntity.IsDeleted)).CurrentValue = true;
                    entry.State = EntityState.Modified;
                    break;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}