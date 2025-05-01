using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors;
public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        //Va a estar pendiente de cambios realizados a cualquier IEntity
        foreach (var entry in context.ChangeTracker.Entries<IEntity>())
        {
            //Si se añade/crea la entidad
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = "Cosmos UwU";
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            //SI se añade/crea o modifica la entidad o si la entidad ha cambiado con el ultimo metodo
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = "Cosmos UwU";
                entry.Entity.LastModified = DateTime.UtcNow;
            }
        }
    }
}

public static class Extensions
{
    //Determina si alguna de las entidades de propiedad (owned entities) relacionadas ha sido agregada o modificada.
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        //Accede a todas las propiedades de navegación de tipo referencia (no colecciones) del objeto entry
        //Usa Any para saber si alguna cumple cierta condición.
        entry.References.Any(r =>
        //Verifica que la entidad relacionada es una entidad propiedad (owned entity).
        //En EF Core, una owned entity no tiene una identidad propia en la base de datos y depende de su entidad principal.
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
        //Verifica si la entidad relacionada ha sido agregada o modificada, es decir, si hay cambios pendientes para guardar.
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}