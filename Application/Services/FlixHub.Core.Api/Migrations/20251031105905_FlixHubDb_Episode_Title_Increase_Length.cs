using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlixHub.Core.Api.Migrations
{
    /// <inheritdoc />
    public partial class FlixHubDb_Episode_Title_Increase_Length : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "Episode",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                comment: "Title of the episode.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldComment: "Title of the episode.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "Episode",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                comment: "Title of the episode.",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldComment: "Title of the episode.");
        }
    }
}
