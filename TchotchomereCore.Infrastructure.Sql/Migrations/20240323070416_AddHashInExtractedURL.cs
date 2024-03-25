using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TchotchomereCore.Infrastructure.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddHashInExtractedURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "ExtractedURL",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "URLHash",
                table: "ExtractedURL",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ExtractedURL_Url",
                table: "ExtractedURL",
                column: "Url",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExtractedURL_Url",
                table: "ExtractedURL");

            migrationBuilder.DropColumn(
                name: "URLHash",
                table: "ExtractedURL");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "ExtractedURL",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
