namespace TchotchomereCore.Domain.Entities;
public class Genre : Entity
{
    public string Name { get; private set; }
    public int? TheMovieDBCode { get; private set; }

#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
    private Genre() { }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
    
    public Genre(string name, int? theMovieDBCode = null, Guid? id = null)
        : base(id)
    {
        Name = name;
        TheMovieDBCode = theMovieDBCode;
    }


    public static List<Genre> MovieGenres { get; } =
        [
            new("Ação e Aventura", 10759, Guid.Parse("9135f939-92dc-4e6d-a78d-3965db200c9c")),
            new("Kids", 10762, Guid.Parse("9f57ca25-e44b-443a-853d-20fb1269878e")),
            new("Ação", 28, Guid.Parse("2bcdafad-1073-4411-b4c1-ebd9b8798e12")),
            new("Aventura", 12, Guid.Parse("e93fb403-a634-431e-96d2-ed18d4d539ec")),
            new("Animação", 16, Guid.Parse("8f745a2c-ed85-4386-914a-01f5868d1ba6")),
            new("Comédia", 35, Guid.Parse("8214caf5-27a5-49b8-af67-d7fbc3e47ce2")),
            new("Crime", 80, Guid.Parse("6a722a42-01b4-4508-b83b-bda626da9546")),
            new("Documentário", 99, Guid.Parse("bd35c568-46b0-4578-8549-52342794910d")),
            new("Drama", 18, Guid.Parse("d8725557-4aa5-40f3-a0be-8c41942d30f7")),
            new("Família", 10751, Guid.Parse("1d99209a-e8dc-479a-9b7e-4ed3ad7a7b01")),
            new("Fantasia", 14, Guid.Parse("7e00b4c3-8446-43f1-9bb9-81589843e219")),
            new("História", 36, Guid.Parse("dacb0dce-28fe-4290-8de7-1f9b587a2914")),
            new("Terror", 27, Guid.Parse("b122bc26-1811-4ef7-b287-454f86aa3903")),
            new("Música", 10402, Guid.Parse("e1399fd5-ae70-4a33-82e0-df55163c7df8")),
            new("Mistério", 9648, Guid.Parse("a220afc6-a007-4fc5-8c26-0e406c088ddd")),
            new("Romance", 10749, Guid.Parse("539c4ee6-ef15-416b-b605-6fb7dda07a6a")),
            new("Ficção Científica", 878, Guid.Parse("8405cc2a-a25f-4499-8e0a-e8231cb0d162")),
            new("Cinema TV", 10770, Guid.Parse("0f90f494-f443-4f61-8030-9e8926a8b097")),
            new("Thriller", 53, Guid.Parse("6b3c66f9-3130-461e-8d01-ebca70bf8bb3")),
            new("Guerra", 10752, Guid.Parse("587e5f54-606c-4b96-ba75-0f2179b1b2ec")),
            new("Faoreste", 37, Guid.Parse("5b10e095-dcdd-400d-8463-a2b60a15a583")),
            new("News", 10763, Guid.Parse("6986c51a-4a7d-4ad0-aea6-2c782eb69fa0")),
            new("Reality", 10764, Guid.Parse("7bd06510-f1d6-449e-bcc9-e83a6543e10e")),
            new("Ficção Científica e Fantasia", 10765, Guid.Parse("b5ebcd0a-5ac5-4b9c-ac04-35f6fc43ca89")),
            new("Soap", 10766, Guid.Parse("fef622cd-fe91-4758-b8bd-e2f65bb7a955")),
            new("Talk", 10767, Guid.Parse("e7b7dcf4-b783-447e-af7d-3ec1af27e0b1")),
            new("Guerra e Política", 10768, Guid.Parse("b25a7dd5-7c43-4ace-8b74-cda8dc0161d1"))
        ];

}
