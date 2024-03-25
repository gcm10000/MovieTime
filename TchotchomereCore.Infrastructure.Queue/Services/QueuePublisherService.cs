using DotNetCore.CAP;
using TchotchomereCore.Domain.Interfaces;

namespace TchotchomereCore.Infrastructure.Queue.Services;

public class QueuePublisherService : IDomainPublisherToOutbox
{
    private readonly ICapPublisher _capBus;

    public QueuePublisherService(ICapPublisher capBus)
    {
        _capBus = capBus;
    }

    public void PublishDomainEvent<TDomainEvent>(TDomainEvent domainEvent)
        where TDomainEvent : IDomainEvent
    {
        var headers = new Dictionary<string, string?>()
        {
            { nameof(domainEvent.EventIdentifier), domainEvent.EventIdentifier },
        };

        var name = typeof(TDomainEvent).Name;

        _capBus.Publish(name, domainEvent, headers);
    }
}
