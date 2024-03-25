using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TchotchomereCore.Infrastructure.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusInMovieEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExtractedUrlId",
                table: "Serie",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Serie",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ExtractedUrlId",
                table: "Film",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Film",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("0f90f494-f443-4f61-8030-9e8926a8b097"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4297));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("1d99209a-e8dc-479a-9b7e-4ed3ad7a7b01"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4270));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("2bcdafad-1073-4411-b4c1-ebd9b8798e12"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4259));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("539c4ee6-ef15-416b-b605-6fb7dda07a6a"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4294));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("587e5f54-606c-4b96-ba75-0f2179b1b2ec"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4300));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("5b10e095-dcdd-400d-8463-a2b60a15a583"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4302));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("6986c51a-4a7d-4ad0-aea6-2c782eb69fa0"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4303));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("6a722a42-01b4-4508-b83b-bda626da9546"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4266));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("6b3c66f9-3130-461e-8d01-ebca70bf8bb3"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4299));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("7bd06510-f1d6-449e-bcc9-e83a6543e10e"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4305));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("7e00b4c3-8446-43f1-9bb9-81589843e219"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4286));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("8214caf5-27a5-49b8-af67-d7fbc3e47ce2"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4264));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("8405cc2a-a25f-4499-8e0a-e8231cb0d162"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4295));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("8f745a2c-ed85-4386-914a-01f5868d1ba6"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4263));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("9135f939-92dc-4e6d-a78d-3965db200c9c"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(3685));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("9f57ca25-e44b-443a-853d-20fb1269878e"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4257));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("a220afc6-a007-4fc5-8c26-0e406c088ddd"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4292));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("b122bc26-1811-4ef7-b287-454f86aa3903"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4289));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("b25a7dd5-7c43-4ace-8b74-cda8dc0161d1"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4311));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("b5ebcd0a-5ac5-4b9c-ac04-35f6fc43ca89"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4306));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("bd35c568-46b0-4578-8549-52342794910d"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4267));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("d8725557-4aa5-40f3-a0be-8c41942d30f7"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4269));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("dacb0dce-28fe-4290-8de7-1f9b587a2914"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4288));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("e1399fd5-ae70-4a33-82e0-df55163c7df8"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4291));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("e7b7dcf4-b783-447e-af7d-3ec1af27e0b1"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4310));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("e93fb403-a634-431e-96d2-ed18d4d539ec"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4261));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("fef622cd-fe91-4758-b8bd-e2f65bb7a955"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 19, 56, 49, 3, DateTimeKind.Utc).AddTicks(4308));

            migrationBuilder.CreateIndex(
                name: "IX_Serie_ExtractedUrlId",
                table: "Serie",
                column: "ExtractedUrlId");

            migrationBuilder.CreateIndex(
                name: "IX_Film_ExtractedUrlId",
                table: "Film",
                column: "ExtractedUrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_Film_ExtractedURL_ExtractedUrlId",
                table: "Film",
                column: "ExtractedUrlId",
                principalTable: "ExtractedURL",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Serie_ExtractedURL_ExtractedUrlId",
                table: "Serie",
                column: "ExtractedUrlId",
                principalTable: "ExtractedURL",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Film_ExtractedURL_ExtractedUrlId",
                table: "Film");

            migrationBuilder.DropForeignKey(
                name: "FK_Serie_ExtractedURL_ExtractedUrlId",
                table: "Serie");

            migrationBuilder.DropIndex(
                name: "IX_Serie_ExtractedUrlId",
                table: "Serie");

            migrationBuilder.DropIndex(
                name: "IX_Film_ExtractedUrlId",
                table: "Film");

            migrationBuilder.DropColumn(
                name: "ExtractedUrlId",
                table: "Serie");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Serie");

            migrationBuilder.DropColumn(
                name: "ExtractedUrlId",
                table: "Film");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Film");

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("0f90f494-f443-4f61-8030-9e8926a8b097"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4923));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("1d99209a-e8dc-479a-9b7e-4ed3ad7a7b01"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4896));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("2bcdafad-1073-4411-b4c1-ebd9b8798e12"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4884));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("539c4ee6-ef15-416b-b605-6fb7dda07a6a"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4920));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("587e5f54-606c-4b96-ba75-0f2179b1b2ec"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4926));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("5b10e095-dcdd-400d-8463-a2b60a15a583"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4928));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("6986c51a-4a7d-4ad0-aea6-2c782eb69fa0"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4929));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("6a722a42-01b4-4508-b83b-bda626da9546"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4891));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("6b3c66f9-3130-461e-8d01-ebca70bf8bb3"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4925));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("7bd06510-f1d6-449e-bcc9-e83a6543e10e"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4931));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("7e00b4c3-8446-43f1-9bb9-81589843e219"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4897));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("8214caf5-27a5-49b8-af67-d7fbc3e47ce2"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4889));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("8405cc2a-a25f-4499-8e0a-e8231cb0d162"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4922));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("8f745a2c-ed85-4386-914a-01f5868d1ba6"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4887));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("9135f939-92dc-4e6d-a78d-3965db200c9c"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4308));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("9f57ca25-e44b-443a-853d-20fb1269878e"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4882));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("a220afc6-a007-4fc5-8c26-0e406c088ddd"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4919));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("b122bc26-1811-4ef7-b287-454f86aa3903"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4900));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("b25a7dd5-7c43-4ace-8b74-cda8dc0161d1"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4937));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("b5ebcd0a-5ac5-4b9c-ac04-35f6fc43ca89"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4933));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("bd35c568-46b0-4578-8549-52342794910d"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4892));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("d8725557-4aa5-40f3-a0be-8c41942d30f7"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4894));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("dacb0dce-28fe-4290-8de7-1f9b587a2914"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4899));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("e1399fd5-ae70-4a33-82e0-df55163c7df8"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4917));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("e7b7dcf4-b783-447e-af7d-3ec1af27e0b1"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4936));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("e93fb403-a634-431e-96d2-ed18d4d539ec"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4886));

            migrationBuilder.UpdateData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: new Guid("fef622cd-fe91-4758-b8bd-e2f65bb7a955"),
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 3, 56, 28, 539, DateTimeKind.Utc).AddTicks(4934));
        }
    }
}
