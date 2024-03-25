using MediatR;
using TchotchomereCore.Application.DTOs.Events;
using TchotchomereCore.Domain.Interfaces;

namespace TchotchomereCore.Application.Handlers.EventHandlers;
public sealed class ExtractedUrlEventHandler : INotificationHandler<ExtractedUrlsEvent>
{
    private readonly IDomainPublisherToOutbox _publisherToOutbox;

    public ExtractedUrlEventHandler(IDomainPublisherToOutbox publisherToOutbox)
    {
        _publisherToOutbox = publisherToOutbox;
    }

    public async Task Handle(ExtractedUrlsEvent notification, CancellationToken cancellationToken)
    {
        _publisherToOutbox.PublishDomainEvent(notification);

        //foreach (var url in URLsWithinWebsite)
        //{
        //    var strUrl = GetFormattedURL(url);

        //    if ((!NewUrls.Contains(strUrl)) && (!OldUrls.Contains(strUrl)))
        //    {
        //        NewUrls.Add(strUrl);
        //    }
        //}
        //NewUrls.Remove(NewUrls[0]);

        //var extractedUrls = notification.Uris
        //    .Select(x => new ExtractedURL(x))
        //    .ToList();

        //await _extractedURLRepository.AddRangeAsync(extractedUrls, cancellationToken);

    }
}
