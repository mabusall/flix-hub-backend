using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlixHub.Core.Api.Migrations
{
    /// <inheritdoc />
    public partial class FlixHubDb_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "SystemUser",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Unique identifier for the SystemUser record.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, comment: "Username of the system user."),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, comment: "Email address of the system user."),
                    KeycloakUserId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Unique identifier for the user in the Keycloak system."),
                    FirstName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, comment: "First name of the system user."),
                    LastName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, comment: "Last name of the system user."),
                    EmailVerificationCode = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "Email verification code for user."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the user is active."),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the user is verified."),
                    Preferences = table.Column<string>(type: "jsonb", nullable: false, comment: "User preferences stored as JSON."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique identifier for the SystemUser."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemUser_Email",
                schema: "public",
                table: "SystemUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemUser_Username",
                schema: "public",
                table: "SystemUser",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemUser",
                schema: "public");
        }
    }
}
