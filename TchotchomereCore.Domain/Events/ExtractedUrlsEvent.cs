using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.Interfaces;

namespace TchotchomereCore.Application.DTOs.Events;

public class ExtractedUrlsEvent : IDomainEvent
{
    public required ExtractedURL CurrentExtractedURL { get; init; }
    public required string EventIdentifier { get; init; }
}
