using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TchotchomereCore.Infrastructure.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddRecordsInGenreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TheMovieDBCode = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "LastModifiedBy", "Name", "TheMovieDBCode", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0f90f494-f443-4f61-8030-9e8926a8b097"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(813), null, null, "Cinema TV", 10770, null },
                    { new Guid("1d99209a-e8dc-479a-9b7e-4ed3ad7a7b01"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(765), null, null, "Família", 10751, null },
                    { new Guid("2bcdafad-1073-4411-b4c1-ebd9b8798e12"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(754), null, null, "Ação", 28, null },
                    { new Guid("539c4ee6-ef15-416b-b605-6fb7dda07a6a"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(810), null, null, "Romance", 10749, null },
                    { new Guid("587e5f54-606c-4b96-ba75-0f2179b1b2ec"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(816), null, null, "Guerra", 10752, null },
                    { new Guid("5b10e095-dcdd-400d-8463-a2b60a15a583"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(818), null, null, "Faoreste", 37, null },
                    { new Guid("6986c51a-4a7d-4ad0-aea6-2c782eb69fa0"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(819), null, null, "News", 10763, null },
                    { new Guid("6a722a42-01b4-4508-b83b-bda626da9546"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(760), null, null, "Crime", 80, null },
                    { new Guid("6b3c66f9-3130-461e-8d01-ebca70bf8bb3"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(815), null, null, "Thriller", 53, null },
                    { new Guid("7bd06510-f1d6-449e-bcc9-e83a6543e10e"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(821), null, null, "Reality", 10764, null },
                    { new Guid("7e00b4c3-8446-43f1-9bb9-81589843e219"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(767), null, null, "Fantasia", 14, null },
                    { new Guid("8214caf5-27a5-49b8-af67-d7fbc3e47ce2"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(759), null, null, "Comédia", 35, null },
                    { new Guid("8405cc2a-a25f-4499-8e0a-e8231cb0d162"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(812), null, null, "Ficção Científica", 878, null },
                    { new Guid("8f745a2c-ed85-4386-914a-01f5868d1ba6"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(757), null, null, "Animação", 16, null },
                    { new Guid("9135f939-92dc-4e6d-a78d-3965db200c9c"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(230), null, null, "Ação e Aventura", 10759, null },
                    { new Guid("9f57ca25-e44b-443a-853d-20fb1269878e"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(752), null, null, "Kids", 10762, null },
                    { new Guid("a220afc6-a007-4fc5-8c26-0e406c088ddd"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(809), null, null, "Mistério", 9648, null },
                    { new Guid("b122bc26-1811-4ef7-b287-454f86aa3903"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(806), null, null, "Terror", 27, null },
                    { new Guid("b25a7dd5-7c43-4ace-8b74-cda8dc0161d1"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(827), null, null, "Guerra e Política", 10768, null },
                    { new Guid("b5ebcd0a-5ac5-4b9c-ac04-35f6fc43ca89"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(822), null, null, "Ficção Científica e Fantasia", 10765, null },
                    { new Guid("bd35c568-46b0-4578-8549-52342794910d"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(762), null, null, "Documentário", 99, null },
                    { new Guid("d8725557-4aa5-40f3-a0be-8c41942d30f7"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(763), null, null, "Drama", 18, null },
                    { new Guid("dacb0dce-28fe-4290-8de7-1f9b587a2914"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(804), null, null, "História", 36, null },
                    { new Guid("e1399fd5-ae70-4a33-82e0-df55163c7df8"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(807), null, null, "Música", 10402, null },
                    { new Guid("e7b7dcf4-b783-447e-af7d-3ec1af27e0b1"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(825), null, null, "Talk", 10767, null },
                    { new Guid("e93fb403-a634-431e-96d2-ed18d4d539ec"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(756), null, null, "Aventura", 12, null },
                    { new Guid("fef622cd-fe91-4758-b8bd-e2f65bb7a955"), new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(824), null, null, "Soap", 10766, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Genre");
        }
    }
}
