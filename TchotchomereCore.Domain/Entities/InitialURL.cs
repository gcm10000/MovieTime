using System.ComponentModel.DataAnnotations.Schema;

namespace TchotchomereCore.Domain.Entities;
public sealed class InitialURL : Entity
{
    public DateTime? ProcessedDateTime { get; private set; } = null!;
    public string Url { get; private set; }
    [NotMapped]
    public Uri Uri { get => new(Url); }

#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
    private InitialURL() { }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.

    public InitialURL(string url)
    {
        Url = url;
    }
}
