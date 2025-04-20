using MediatR;

namespace Ordering.Domain.Abstractions;
//Usamos MediatR para poder entregar los eventos del dominio mendiante los handlers de MediatR
public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName;
}