using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlixHub.Core.Api.Migrations
{
    /// <inheritdoc />
    public partial class FlixHubDb_Increase_ContentCast_Character_Length : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Character",
                schema: "public",
                table: "ContentCast",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                comment: "Character name played by the actor.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Character name played by the actor.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Character",
                schema: "public",
                table: "ContentCast",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Character name played by the actor.",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true,
                oldComment: "Character name played by the actor.");
        }
    }
}
