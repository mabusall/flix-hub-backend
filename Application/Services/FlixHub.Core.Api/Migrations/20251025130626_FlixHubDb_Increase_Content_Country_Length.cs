using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlixHub.Core.Api.Migrations
{
    /// <inheritdoc />
    public partial class FlixHubDb_Increase_Content_Country_Length : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Country",
                schema: "public",
                table: "Content",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "ISO 3166-1 country code.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "ISO 3166-1 country code.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Country",
                schema: "public",
                table: "Content",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "ISO 3166-1 country code.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "ISO 3166-1 country code.");
        }
    }
}
