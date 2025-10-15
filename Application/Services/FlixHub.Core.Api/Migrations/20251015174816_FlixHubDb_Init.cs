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
                name: "Content",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for Content.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TmdbId = table.Column<int>(type: "integer", nullable: false, comment: "TMDb ID (unique for movie/tv)."),
                    ImdbId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "IMDb ID."),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "Content type: Movie or TV."),
                    Title = table.Column<string>(type: "text", nullable: false, comment: "Display title of the content."),
                    OriginalTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Original title or name in original language."),
                    Overview = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Summary / description."),
                    Awards = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Awards wining."),
                    OriginalLanguage = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "ISO 639-1 language code."),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: true, comment: "Movie: release_date / TV: first_air_date."),
                    Status = table.Column<int>(type: "integer", nullable: true, comment: "Release status: Released, Ended, Returning Series."),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "ISO 3166-1 country code."),
                    Runtime = table.Column<int>(type: "integer", nullable: true, comment: "Runtime in minutes (movie or avg episode runtime)."),
                    Popularity = table.Column<decimal>(type: "numeric(12,6)", nullable: true, comment: "TMDb popularity score."),
                    VoteAverage = table.Column<decimal>(type: "numeric(4,2)", nullable: true, comment: "TMDb average vote rating."),
                    VoteCount = table.Column<int>(type: "integer", nullable: true, comment: "TMDb vote count."),
                    Budget = table.Column<long>(type: "bigint", nullable: true, comment: "Movie/TV Budget."),
                    PosterPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Poster image path or URL."),
                    BackdropPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Backdrop image path or URL."),
                    LogoPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Logo image path or URL."),
                    IsAdult = table.Column<bool>(type: "boolean", nullable: false),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for Content."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentSyncLog",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for ContentSyncLog.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "Content type being synced: 1=Movie, 2=Series."),
                    Year = table.Column<int>(type: "integer", nullable: false, comment: "Year of the sync batch."),
                    Month = table.Column<int>(type: "integer", nullable: false, comment: "Month of the sync batch (1–12)."),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the sync for this year/month/type is completed."),
                    LastCompletedPage = table.Column<int>(type: "integer", nullable: false, comment: "Last successfully completed page number for this sync batch."),
                    TotalPages = table.Column<int>(type: "integer", nullable: true, comment: "Total pages available for this sync batch (from TMDb)."),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "Optional notes or error details for debugging."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for ContentSyncLog."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentSyncLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyApiUsage",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for DailyApiUsage.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date of the API usage tracking (daily basis)."),
                    ContentType = table.Column<int>(type: "integer", nullable: false, comment: "Content type for which requests were made: 1=Movie, 2=Series."),
                    RequestCount = table.Column<int>(type: "integer", nullable: false, comment: "Number of API requests made for this content type on this date."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for DailyApiUsage."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyApiUsage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for Genre.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TmdbReferenceId = table.Column<int>(type: "integer", nullable: false, comment: "Optional TMDb genre id for mapping."),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Name of the genre (Drama, Action, etc.)."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for Genre."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for Person.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TmdbId = table.Column<long>(type: "bigint", nullable: false, comment: "TMDb ID (unique for person)."),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, comment: "Full name of the person."),
                    Gender = table.Column<int>(type: "integer", nullable: false, comment: "Gender: 0=Unknown, 1=Female, 2=Male, 3=NonBinary."),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true, comment: "Birth date of the person."),
                    DeathDate = table.Column<DateTime>(type: "date", nullable: true, comment: "Death date of the person."),
                    KnownForDepartment = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Department this person is best known for (Acting, Directing, Writing)."),
                    Biography = table.Column<string>(type: "text", nullable: true, comment: "Person Biography."),
                    BirthPlace = table.Column<string>(type: "text", nullable: true, comment: "Birth place of the person."),
                    PersonalPhoto = table.Column<string>(type: "text", nullable: true, comment: "Profile image path from TMDb."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for Person."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemUser",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the SystemUser record.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "varchar", maxLength: 150, nullable: false, comment: "Username of the system user."),
                    Email = table.Column<string>(type: "varchar", maxLength: 150, nullable: false, comment: "Email address of the system user."),
                    KeycloakUserId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Unique identifier for the user in the Keycloak system."),
                    FirstName = table.Column<string>(type: "varchar", maxLength: 150, nullable: false, comment: "First name of the system user."),
                    LastName = table.Column<string>(type: "varchar", maxLength: 150, nullable: false, comment: "Last name of the system user."),
                    EmailVerificationCode = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "Email verification code for user."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the user is active."),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the user is verified."),
                    Preferences = table.Column<string>(type: "jsonb", nullable: true, comment: "User preferences stored as JSON."),
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

            migrationBuilder.CreateTable(
                name: "ContentImage",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for ContentImage.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Content entity."),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "Image type: 1=Poster, 2=Backdrop, 3=Logo, 4=Still."),
                    FilePath = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false, comment: "File path for the image from TMDb."),
                    Language = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true, comment: "Language code (iso_639_1)."),
                    Width = table.Column<int>(type: "integer", nullable: false, comment: "Image width in pixels."),
                    Height = table.Column<int>(type: "integer", nullable: false, comment: "Image height in pixels."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for ContentImage."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentImage_Content_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "public",
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentRating",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for ContentRating.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Content entity."),
                    Source = table.Column<int>(type: "integer", nullable: false, comment: "Rating source: 1=IMDb, 2=RottenTomatoes, 3=Metacritic."),
                    Value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Rating value string from OMDb, e.g. '8.3/10', '98%', '88/100'."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for ContentRating."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentRating_Content_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "public",
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentSeason",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for Season.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Content entity (TV series)."),
                    SeasonNumber = table.Column<int>(type: "integer", nullable: false, comment: "Season number, starting at 1."),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Optional title of the season."),
                    Overview = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Overview/description of the season."),
                    AirDate = table.Column<DateTime>(type: "date", nullable: true, comment: "First air date of the season."),
                    EpisodeCount = table.Column<int>(type: "integer", nullable: true, comment: "Number of episodes in this season (if known)."),
                    PosterPath = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Poster image path from TMDb."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for Season."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentSeason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentSeason_Content_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "public",
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentVideo",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for ContentVideo.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Content entity."),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "Video type: 1=Trailer, 2=Teaser, 3=Clip, 4=Featurette."),
                    Site = table.Column<int>(type: "integer", nullable: false, comment: "Video site: 1=YouTube, 2=Vimeo."),
                    Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "External video key (e.g., YouTube video id)."),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "Title of the video."),
                    IsOfficial = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the video is an official release."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for ContentVideo."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentVideo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentVideo_Content_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "public",
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentGenre",
                schema: "public",
                columns: table => new
                {
                    ContentId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Content (movie or TV)."),
                    GenreId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Genre."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for Genre.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for Genre."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentGenre", x => new { x.ContentId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_ContentGenre_Content_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "public",
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentGenre_Genre_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "public",
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentCast",
                schema: "public",
                columns: table => new
                {
                    ContentId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Content (movie or TV)."),
                    PersonId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Person."),
                    CreditId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Credit ID from TMDb."),
                    Character = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Character name played by the actor."),
                    Order = table.Column<int>(type: "integer", nullable: true, comment: "Billing order of the cast member from TMDb."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for ContentCast.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for ContentCast."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentCast", x => new { x.ContentId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_ContentCast_Content_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "public",
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentCast_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "public",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentCrew",
                schema: "public",
                columns: table => new
                {
                    ContentId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Content (movie or TV)."),
                    PersonId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Person."),
                    CreditId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Credit ID from TMDb."),
                    Department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Department this person worked in (Directing, Writing, Production)."),
                    Job = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Specific job title (Director, Writer, Producer)."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for ContentCrew.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for ContentCrew."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Username or identifier of the user who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentCrew", x => new { x.ContentId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_ContentCrew_Content_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "public",
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentCrew_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "public",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Watchlist",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for Watchlist.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to SystemUser entity."),
                    ContentId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Content entity."),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the content was added to the watchlist."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for Watchlist."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchlist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Watchlist_Content_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "public",
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Watchlist_SystemUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "SystemUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episode",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for Episode.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeasonId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Season entity."),
                    EpisodeNumber = table.Column<int>(type: "integer", nullable: false, comment: "Episode number within the season."),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Title of the episode."),
                    Overview = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Overview/description of the episode."),
                    AirDate = table.Column<DateTime>(type: "date", nullable: true, comment: "Air date of the episode."),
                    Runtime = table.Column<int>(type: "integer", nullable: true, comment: "Runtime of the episode in minutes."),
                    StillPath = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Still image path from TMDb."),
                    VoteAverage = table.Column<decimal>(type: "numeric(4,2)", nullable: true, comment: "Average vote score for the episode."),
                    VoteCount = table.Column<int>(type: "integer", nullable: true, comment: "Number of votes for the episode."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for Episode."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episode_ContentSeason_SeasonId",
                        column: x => x.SeasonId,
                        principalSchema: "public",
                        principalTable: "ContentSeason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EpisodeCrew",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Internal primary key for EpisodeCrew.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EpisodeId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Episode entity."),
                    PersonId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key to Person entity."),
                    CreditId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Credit ID from TMDb."),
                    Department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Department of the crew member (Directing, Writing, etc.)."),
                    Job = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Specific job title of the crew member."),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique UUID identifier for EpisodeCrew."),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Date and time when the record was created."),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who created the record."),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Date and time when the record was last modified."),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "User who last modified the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpisodeCrew", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpisodeCrew_Episode_EpisodeId",
                        column: x => x.EpisodeId,
                        principalSchema: "public",
                        principalTable: "Episode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EpisodeCrew_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "public",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Content_TmdbId",
                schema: "public",
                table: "Content",
                column: "TmdbId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentCast_PersonId",
                schema: "public",
                table: "ContentCast",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentCrew_PersonId",
                schema: "public",
                table: "ContentCrew",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentGenre_GenreId",
                schema: "public",
                table: "ContentGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentImage_ContentId",
                schema: "public",
                table: "ContentImage",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentRating_ContentId",
                schema: "public",
                table: "ContentRating",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentSeason_ContentId_SeasonNumber",
                schema: "public",
                table: "ContentSeason",
                columns: new[] { "ContentId", "SeasonNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentSyncLog_Type_Year_Month",
                schema: "public",
                table: "ContentSyncLog",
                columns: new[] { "Type", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentVideo_ContentId",
                schema: "public",
                table: "ContentVideo",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyApiUsage_Date_ContentType",
                schema: "public",
                table: "DailyApiUsage",
                columns: new[] { "Date", "ContentType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Episode_SeasonId_EpisodeNumber",
                schema: "public",
                table: "Episode",
                columns: new[] { "SeasonId", "EpisodeNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeCrew_EpisodeId",
                schema: "public",
                table: "EpisodeCrew",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeCrew_PersonId",
                schema: "public",
                table: "EpisodeCrew",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_TmdbReferenceId",
                schema: "public",
                table: "Genre",
                column: "TmdbReferenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_TmdbId",
                schema: "public",
                table: "Person",
                column: "TmdbId",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Watchlist_ContentId",
                schema: "public",
                table: "Watchlist",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Watchlist_UserId_ContentId",
                schema: "public",
                table: "Watchlist",
                columns: new[] { "UserId", "ContentId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentCast",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentCrew",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentGenre",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentImage",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentRating",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentSyncLog",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentVideo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "DailyApiUsage",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EpisodeCrew",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Watchlist",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Genre",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Episode",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SystemUser",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentSeason",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Content",
                schema: "public");
        }
    }
}
