using MediatR;

namespace TchotchomereCore.Domain.Interfaces;
public interface IDomainEvent : INotification
{
    string EventIdentifier { get; }
}
