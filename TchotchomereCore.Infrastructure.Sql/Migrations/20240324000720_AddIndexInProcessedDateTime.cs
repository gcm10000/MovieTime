using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TchotchomereCore.Infrastructure.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexInProcessedDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExtractedURL_ProcessedDateTime",
                table: "ExtractedURL",
                column: "ProcessedDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExtractedURL_ProcessedDateTime",
                table: "ExtractedURL");
        }
    }
}
