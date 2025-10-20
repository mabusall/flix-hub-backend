namespace FlixHub.Core.Api.Repository;

class SystemUserConfiguration : IEntityTypeConfiguration<SystemUser>
{
    public void Configure(EntityTypeBuilder<SystemUser> builder)
    {
        builder.HasIndex(p => p.Username).IsUnique();
        builder.HasIndex(p => p.Email).IsUnique();

        builder.Property(p => p.Id).HasComment("Unique identifier for the SystemUser record.");
        builder.Property(p => p.Uuid).HasComment("Unique identifier for the SystemUser.");

        builder.Property(p => p.Username)
            .HasComment("Username of the system user.")
            .HasConversion<DbEncryptionProvider>();

        builder.Property(p => p.Email)
            .HasComment("Email address of the system user.")
            .HasConversion<DbEncryptionProvider>();

        builder.Property(p => p.Password)
            .HasComment("Hashed password of the system user.")
            .HasConversion<DbEncryptionProvider>();

        builder.Property(p => p.FirstName)
            .HasComment("First name of the system user.")
            .HasConversion<DbEncryptionProvider>();

        builder.Property(p => p.LastName)
            .HasComment("Last name of the system user.")
            .HasConversion<DbEncryptionProvider>();

        builder.Property(p => p.EmailVerificationCode)
            .HasColumnType("varchar(200)")
            .HasComment("Email verification code for user.");

        builder.Property(p => p.IsActive)
            .HasComment("Indicates whether the user is active.");

        builder.Property(p => p.IsVerified)
            .HasComment("Indicates whether the user is verified.");

        builder.Property(p => p.Preferences)
            .HasComment("User preferences stored as JSON.")
            .HasConversion(v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                           v => JsonSerializer.Deserialize<UserPreferencesDto>(v, JsonSerializerOptions.Default)!);

        builder.Property(p => p.Created)
            .HasComment("Date and time when the record was created.");
        builder.Property(p => p.CreatedBy)
            .HasComment("Username or identifier of the user who created the record.");
        builder.Property(p => p.LastModified)
            .HasComment("Date and time when the record was last modified.");
        builder.Property(p => p.LastModifiedBy)
            .HasComment("Username or identifier of the user who last modified the record.");
    }
}

class ContentConfiguration : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> builder)
    {
        // Primary keys & identifiers
        builder.Property(p => p.Id)
            .HasComment("Internal primary key for Content.");

        builder.Property(p => p.Uuid)
            .HasComment("Unique UUID identifier for Content.");

        // Core identifiers
        builder.Property(p => p.TmdbId)
            .HasComment("TMDb ID (unique for movie/tv).");

        builder.Property(p => p.ImdbId)
            .HasComment("IMDb ID.");

        // Type (Movie/TV)
        builder.Property(p => p.Type)
            .HasConversion<int>()
            .HasComment("Content type: Movie or TV.");

        // Titles & descriptions
        builder.Property(p => p.Title)
            .HasComment("Display title of the content.");

        builder.Property(p => p.OriginalTitle)
            .HasComment("Original title or name in original language.");

        builder.Property(p => p.Budget)
            .HasComment("Movie/TV Budget.");

        builder.Property(p => p.Awards)
            .HasComment("Awards wining.");

        builder.Property(p => p.Overview)
            .HasComment("Summary / description.");

        builder.Property(p => p.OriginalLanguage)
            .HasComment("ISO 639-1 language code.");

        // Dates
        builder.Property(p => p.ReleaseDate)
            .HasColumnType("date")
            .HasComment("Movie: release_date / TV: first_air_date.");

        // Status
        builder.Property(p => p.Status)
            .HasConversion<int>()
            .HasComment("Release status: Released, Ended, Returning Series.");

        // Country
        builder.Property(p => p.Country)
            .HasComment("ISO 3166-1 country code.");

        // Runtime
        builder.Property(p => p.Runtime)
            .HasComment("Runtime in minutes (movie or avg episode runtime).");

        // Popularity & votes
        builder.Property(p => p.Popularity)
            .HasComment("TMDb popularity score.");

        builder.Property(p => p.VoteAverage)
            .HasComment("TMDb average vote rating.");

        builder.Property(p => p.VoteCount)
            .HasComment("TMDb vote count.");

        // Media paths
        builder.Property(p => p.PosterPath)
            .HasComment("Poster image path or URL.");
        builder.Property(p => p.BackdropPath)
            .HasComment("Backdrop image path or URL.");
        builder.Property(p => p.LogoPath)
            .HasComment("Logo image path or URL.");

        // Auditable entity fields
        builder.Property(p => p.Created)
            .HasComment("Date and time when the record was created.");
        builder.Property(p => p.CreatedBy)
            .HasComment("Username or identifier of the user who created the record.");
        builder.Property(p => p.LastModified)
            .HasComment("Date and time when the record was last modified.");
        builder.Property(p => p.LastModifiedBy)
            .HasComment("Username or identifier of the user who last modified the record.");

        // Indexes
        builder.HasIndex(p => p.TmdbId).IsUnique();
    }
}

class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasIndex(g => g.TmdbReferenceId).IsUnique();

        // Primary keys & identifiers
        builder.Property(p => p.Id)
            .HasComment("Internal primary key for Genre.");
        builder.Property(p => p.Uuid)
            .HasComment("Unique UUID identifier for Genre.");
        builder.Property(g => g.TmdbReferenceId)
            .HasComment("Optional TMDb genre id for mapping.");
        builder.Property(g => g.Name)
            .HasComment("Name of the genre (Drama, Action, etc.).");
        // Auditable entity fields
        builder.Property(p => p.Created)
            .HasComment("Date and time when the record was created.");
        builder.Property(p => p.CreatedBy)
            .HasComment("Username or identifier of the user who created the record.");
        builder.Property(p => p.LastModified)
            .HasComment("Date and time when the record was last modified.");
        builder.Property(p => p.LastModifiedBy)
            .HasComment("Username or identifier of the user who last modified the record.");
    }
}

class ContentGenreConfiguration : IEntityTypeConfiguration<ContentGenre>
{
    public void Configure(EntityTypeBuilder<ContentGenre> builder)
    {
        builder.HasKey(cg => new { cg.ContentId, cg.GenreId });

        builder.HasOne<Content>()
            .WithMany(c => c.Genres)
            .HasForeignKey(cg => cg.ContentId);

        builder.HasOne(cg => cg.Genre)
            .WithMany()
            .HasForeignKey(cg => cg.GenreId);

        builder.Property(p => p.Id)
            .HasComment("Internal primary key for Genre.");
        builder.Property(p => p.Uuid)
            .HasComment("Unique UUID identifier for Genre.");
        builder.Property(cg => cg.ContentId)
            .HasComment("Foreign key to Content (movie or TV).");
        builder.Property(cg => cg.GenreId)
            .HasComment("Foreign key to Genre.");
        // Auditable entity fields
        builder.Property(p => p.Created)
            .HasComment("Date and time when the record was created.");
        builder.Property(p => p.CreatedBy)
            .HasComment("Username or identifier of the user who created the record.");
        builder.Property(p => p.LastModified)
            .HasComment("Date and time when the record was last modified.");
        builder.Property(p => p.LastModifiedBy)
            .HasComment("Username or identifier of the user who last modified the record.");
    }
}

class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(p => p.Id)
            .HasComment("Internal primary key for Person.");
        builder.Property(p => p.Uuid)
            .HasComment("Unique UUID identifier for Person.");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasColumnType("varchar(150)")
            .HasComment("Full name of the person.");

        builder.Property(p => p.Gender)
            .HasConversion<int>() // store as int (0,1,2,3)
            .HasComment("Gender: 0=Unknown, 1=Female, 2=Male, 3=NonBinary.");

        builder.Property(p => p.BirthDate)
            .HasColumnType("date")
            .HasComment("Birth date of the person.");
        builder.Property(p => p.DeathDate)
            .HasColumnType("date")
            .HasComment("Death date of the person.");

        builder.Property(p => p.KnownForDepartment)
            .HasComment("Department this person is best known for (Acting, Directing, Writing).");

        builder.Property(p => p.Biography)
            .HasComment("Person Biography.");

        builder.Property(p => p.PersonalPhoto)
            .HasComment("Profile image path from TMDb.");

        builder.Property(p => p.BirthPlace)
            .HasComment("Birth place of the person.");

        builder.Property(p => p.TmdbId)
            .HasComment("TMDb ID (unique for person).");

        // Indexes
        builder.HasIndex(p => p.TmdbId).IsUnique();

        // Audit fields
        builder.Property(p => p.Created).HasComment("Date and time when the record was created.");
        builder.Property(p => p.CreatedBy).HasComment("User who created the record.");
        builder.Property(p => p.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(p => p.LastModifiedBy).HasComment("User who last modified the record.");
    }
}

class ContentCastConfiguration : IEntityTypeConfiguration<ContentCast>
{
    public void Configure(EntityTypeBuilder<ContentCast> builder)
    {
        // Primary key includes the credit
        builder.HasKey(cc => new { cc.ContentId, cc.PersonId, cc.CreditId });

        // Useful non-unique index to fetch “all casts for a person in a content”
        builder.HasIndex(cc => new { cc.ContentId, cc.PersonId }).IsUnique(false);

        builder.HasOne(cc => cc.Person)
            .WithMany()
            .HasForeignKey(cc => cc.PersonId);

        builder.HasOne<Content>()
            .WithMany(c => c.Casts)
            .HasForeignKey(cc => cc.ContentId);

        builder.Property(cc => cc.ContentId)
            .HasComment("Foreign key to Content (movie or TV).");
        builder.Property(cc => cc.PersonId)
            .HasComment("Foreign key to Person.");
        builder.Property(cc => cc.CreditId)
            .HasComment("Credit ID from TMDb.");
        builder.Property(cc => cc.Character)
            .HasComment("Character name played by the actor.");
        builder.Property(cc => cc.Order)
            .HasComment("Billing order of the cast member from TMDb.");

        // Audit fields
        builder.Property(p => p.Id).HasComment("Internal primary key for ContentCast.");
        builder.Property(p => p.Uuid).HasComment("Unique UUID identifier for ContentCast.");
        builder.Property(p => p.Created).HasComment("Date and time when the record was created.");
        builder.Property(p => p.CreatedBy).HasComment("Username or identifier of the user who created the record.");
        builder.Property(p => p.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(p => p.LastModifiedBy).HasComment("Username or identifier of the user who last modified the record.");
    }
}

class ContentCrewConfiguration : IEntityTypeConfiguration<ContentCrew>
{
    public void Configure(EntityTypeBuilder<ContentCrew> builder)
    {
        // Primary key includes the credit
        builder.HasKey(cc => new { cc.ContentId, cc.PersonId, cc.CreditId });

        // Useful non-unique index to fetch “all casts for a person in a content”
        builder.HasIndex(cc => new { cc.ContentId, cc.PersonId }).IsUnique(false);

        builder.HasOne(cc => cc.Person)
            .WithMany()
            .HasForeignKey(cc => cc.PersonId);
        builder.HasOne<Content>()
            .WithMany(c => c.Crews)
            .HasForeignKey(cc => cc.ContentId);

        builder.Property(cc => cc.ContentId)
            .HasComment("Foreign key to Content (movie or TV).");
        builder.Property(cc => cc.PersonId)
            .HasComment("Foreign key to Person.");
        builder.Property(cc => cc.CreditId)
            .HasComment("Credit ID from TMDb.");
        builder.Property(cc => cc.Department)
            .HasMaxLength(100)
            .HasComment("Department this person worked in (Directing, Writing, Production).");
        builder.Property(cc => cc.Job)
            .HasMaxLength(100)
            .HasComment("Specific job title (Director, Writer, Producer).");

        // Audit fields
        builder.Property(p => p.Id).HasComment("Internal primary key for ContentCrew.");
        builder.Property(p => p.Uuid).HasComment("Unique UUID identifier for ContentCrew.");
        builder.Property(p => p.Created).HasComment("Date and time when the record was created.");
        builder.Property(p => p.CreatedBy).HasComment("Username or identifier of the user who created the record.");
        builder.Property(p => p.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(p => p.LastModifiedBy).HasComment("Username or identifier of the user who last modified the record.");
    }
}

class EpisodeCrewConfiguration : IEntityTypeConfiguration<EpisodeCrew>
{
    public void Configure(EntityTypeBuilder<EpisodeCrew> builder)
    {
        builder.Property(ec => ec.Id)
            .HasComment("Internal primary key for EpisodeCrew.");
        builder.Property(ec => ec.Uuid)
            .HasComment("Unique UUID identifier for EpisodeCrew.");

        // Primary key includes the credit
        builder.HasKey(cc => new { cc.EpisodeId, cc.PersonId, cc.CreditId });

        // Useful non-unique index to fetch “all casts for a person in a content”
        builder.HasIndex(cc => new { cc.EpisodeId, cc.PersonId }).IsUnique(false);

        builder.Property(ec => ec.EpisodeId)
            .HasComment("Foreign key to Episode entity.");
        builder.Property(ec => ec.PersonId)
            .HasComment("Foreign key to Person entity.");
        builder.Property(cc => cc.CreditId)
            .HasComment("Credit ID from TMDb.");
        builder.Property(ec => ec.Department)
            .HasComment("Department of the crew member (Directing, Writing, etc.).");

        builder.Property(ec => ec.Job)
            .HasComment("Specific job title of the crew member.");

        // Relationships
        builder.HasOne<Episode>()
            .WithMany(e => e.Crews)
            .HasForeignKey(ec => ec.EpisodeId);

        builder.HasOne(ec => ec.Person)
            .WithMany()
            .HasForeignKey(ec => ec.PersonId);

        // Audit fields
        builder.Property(ec => ec.Created).HasComment("Date and time when the record was created.");
        builder.Property(ec => ec.CreatedBy).HasComment("User who created the record.");
        builder.Property(ec => ec.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(ec => ec.LastModifiedBy).HasComment("User who last modified the record.");
    }
}

class ContentRatingConfiguration : IEntityTypeConfiguration<ContentRating>
{
    public void Configure(EntityTypeBuilder<ContentRating> builder)
    {
        builder.Property(cr => cr.Id)
            .HasComment("Internal primary key for ContentRating.");
        builder.Property(cr => cr.Uuid)
            .HasComment("Unique UUID identifier for ContentRating.");

        builder.Property(cr => cr.ContentId)
            .HasComment("Foreign key to Content entity.");

        builder.Property(cr => cr.Source)
            .HasConversion<int>() // store enum as int in DB
            .HasComment("Rating source: 1=IMDb, 2=RottenTomatoes, 3=Metacritic.");

        builder.Property(cr => cr.Value)
            .HasComment("Rating value string from OMDb, e.g. '8.3/10', '98%', '88/100'.");

        // Relationships
        builder.HasOne<Content>()
            .WithMany(c => c.Ratings)
            .HasForeignKey(cr => cr.ContentId);

        // Audit fields
        builder.Property(cr => cr.Created).HasComment("Date and time when the record was created.");
        builder.Property(cr => cr.CreatedBy).HasComment("User who created the record.");
        builder.Property(cr => cr.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(cr => cr.LastModifiedBy).HasComment("User who last modified the record.");
    }
}

class ContentImageConfiguration : IEntityTypeConfiguration<ContentImage>
{
    public void Configure(EntityTypeBuilder<ContentImage> builder)
    {
        builder.Property(ci => ci.Id)
            .HasComment("Internal primary key for ContentImage.");
        builder.Property(ci => ci.Uuid)
            .HasComment("Unique UUID identifier for ContentImage.");

        builder.Property(ci => ci.ContentId)
            .HasComment("Foreign key to Content entity.");

        builder.Property(ci => ci.Type)
            .HasConversion<int>() // store enum as int
            .HasComment("Image type: 1=Poster, 2=Backdrop, 3=Logo, 4=Still.");

        builder.Property(ci => ci.FilePath)
            .HasComment("File path for the image from TMDb.");

        builder.Property(ci => ci.Language)
            .HasComment("Language code (iso_639_1).");

        builder.Property(ci => ci.Width)
            .HasComment("Image width in pixels.");
        builder.Property(ci => ci.Height)
            .HasComment("Image height in pixels.");

        // Relationships
        builder.HasOne<Content>()
            .WithMany(c => c.Images)
            .HasForeignKey(ci => ci.ContentId);

        // Audit fields
        builder.Property(ci => ci.Created).HasComment("Date and time when the record was created.");
        builder.Property(ci => ci.CreatedBy).HasComment("User who created the record.");
        builder.Property(ci => ci.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(ci => ci.LastModifiedBy).HasComment("User who last modified the record.");
    }
}
class ContentVideoConfiguration : IEntityTypeConfiguration<ContentVideo>
{
    public void Configure(EntityTypeBuilder<ContentVideo> builder)
    {
        builder.Property(cv => cv.Id)
            .HasComment("Internal primary key for ContentVideo.");
        builder.Property(cv => cv.Uuid)
            .HasComment("Unique UUID identifier for ContentVideo.");

        builder.Property(cv => cv.ContentId)
            .HasComment("Foreign key to Content entity.");

        builder.Property(cv => cv.Type)
            .HasConversion<int>()
            .HasComment("Video type: 1=Trailer, 2=Teaser, 3=Clip, 4=Featurette.");

        builder.Property(cv => cv.Site)
            .HasConversion<int>()
            .HasComment("Video site: 1=YouTube, 2=Vimeo.");

        builder.Property(cv => cv.Key)
            .HasComment("External video key (e.g., YouTube video id).");

        builder.Property(cv => cv.Name)
            .HasComment("Title of the video.");

        builder.Property(cv => cv.IsOfficial)
            .HasComment("Indicates whether the video is an official release.");

        // Relationships
        builder.HasOne<Content>()
            .WithMany(c => c.Videos)
            .HasForeignKey(cv => cv.ContentId);

        // Audit fields
        builder.Property(cv => cv.Created).HasComment("Date and time when the record was created.");
        builder.Property(cv => cv.CreatedBy).HasComment("User who created the record.");
        builder.Property(cv => cv.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(cv => cv.LastModifiedBy).HasComment("User who last modified the record.");
    }
}

class ContentSeasonConfiguration : IEntityTypeConfiguration<ContentSeason>
{
    public void Configure(EntityTypeBuilder<ContentSeason> builder)
    {
        builder.Property(s => s.Id)
            .HasComment("Internal primary key for Season.");
        builder.Property(s => s.Uuid)
            .HasComment("Unique UUID identifier for Season.");

        builder.Property(s => s.ContentId)
            .HasComment("Foreign key to Content entity (TV series).");

        builder.Property(s => s.SeasonNumber)
            .HasComment("Season number, starting at 1.");

        builder.Property(s => s.Title)
            .HasComment("Optional title of the season.");

        builder.Property(s => s.Overview)
            .HasComment("Overview/description of the season.");

        builder.Property(s => s.AirDate)
            .HasColumnType("date")
            .HasComment("First air date of the season.");

        builder.Property(s => s.EpisodeCount)
            .HasComment("Number of episodes in this season (if known).");

        builder.Property(s => s.PosterPath)
            .HasComment("Poster image path from TMDb.");

        // Relationships
        builder.HasOne<Content>()
            .WithMany(c => c.Seasons)
            .HasForeignKey(s => s.ContentId);

        // Indexes
        builder.HasIndex(s => new { s.ContentId, s.SeasonNumber })
            .IsUnique();

        // Audit fields
        builder.Property(s => s.Created).HasComment("Date and time when the record was created.");
        builder.Property(s => s.CreatedBy).HasComment("User who created the record.");
        builder.Property(s => s.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(s => s.LastModifiedBy).HasComment("User who last modified the record.");
    }
}

class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.Property(e => e.Id)
            .HasComment("Internal primary key for Episode.");
        builder.Property(e => e.Uuid)
            .HasComment("Unique UUID identifier for Episode.");

        builder.Property(e => e.SeasonId)
            .HasComment("Foreign key to Season entity.");

        builder.Property(e => e.EpisodeNumber)
            .HasComment("Episode number within the season.");

        builder.Property(e => e.Title)
            .HasComment("Title of the episode.");

        builder.Property(e => e.Overview)
            .HasComment("Overview/description of the episode.");

        builder.Property(e => e.AirDate)
            .HasColumnType("date")
            .HasComment("Air date of the episode.");

        builder.Property(e => e.Runtime)
            .HasComment("Runtime of the episode in minutes.");

        builder.Property(e => e.StillPath)
            .HasComment("Still image path from TMDb.");

        builder.Property(e => e.VoteAverage)
            .HasComment("Average vote score for the episode.");
        builder.Property(e => e.VoteCount)
            .HasComment("Number of votes for the episode.");

        // Relationships
        builder.HasOne<ContentSeason>()
            .WithMany(s => s.Episodes)
            .HasForeignKey(e => e.SeasonId);

        // Indexes
        builder.HasIndex(e => new { e.SeasonId, e.EpisodeNumber })
            .IsUnique();

        // Audit fields
        builder.Property(e => e.Created).HasComment("Date and time when the record was created.");
        builder.Property(e => e.CreatedBy).HasComment("User who created the record.");
        builder.Property(e => e.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(e => e.LastModifiedBy).HasComment("User who last modified the record.");
    }
}

class WatchlistConfiguration : IEntityTypeConfiguration<Watchlist>
{
    public void Configure(EntityTypeBuilder<Watchlist> builder)
    {
        builder.Property(w => w.Id)
            .HasComment("Internal primary key for Watchlist.");
        builder.Property(w => w.Uuid)
            .HasComment("Unique UUID identifier for Watchlist.");

        builder.Property(w => w.UserId)
            .HasComment("Foreign key to SystemUser entity.");
        builder.Property(w => w.ContentId)
            .HasComment("Foreign key to Content entity.");

        builder.Property(w => w.AddedAt)
            .HasComment("Date and time when the content was added to the watchlist.");

        // Relationships
        builder.HasOne<SystemUser>()
            .WithMany(u => u.Watchlist)
            .HasForeignKey(w => w.UserId);

        builder.HasOne(w => w.Content)
            .WithMany()
            .HasForeignKey(w => w.ContentId);

        // Unique constraint: a user cannot add the same content twice
        builder
            .HasIndex(w => new { w.UserId, w.ContentId })
            .IsUnique();

        // Audit fields
        builder.Property(w => w.Created).HasComment("Date and time when the record was created.");
        builder.Property(w => w.CreatedBy).HasComment("User who created the record.");
        builder.Property(w => w.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(w => w.LastModifiedBy).HasComment("User who last modified the record.");
    }
}

class ContentSyncLogConfiguration : IEntityTypeConfiguration<ContentSyncLog>
{
    public void Configure(EntityTypeBuilder<ContentSyncLog> builder)
    {
        builder.Property(csl => csl.Id)
            .HasComment("Internal primary key for ContentSyncLog.");
        builder.Property(csl => csl.Uuid)
            .HasComment("Unique UUID identifier for ContentSyncLog.");

        builder.Property(csl => csl.Type)
            .HasConversion<int>()
            .HasComment("Content type being synced: 1=Movie, 2=Series.");

        builder.Property(csl => csl.Year)
            .HasComment("Year of the sync batch.");

        builder.Property(csl => csl.Month)
            .HasComment("Month of the sync batch (1–12).");

        builder.Property(csl => csl.IsCompleted)
            .HasComment("Indicates whether the sync for this year/month/type is completed.");

        builder.Property(csl => csl.LastCompletedPage)
            .HasComment("Last successfully completed page number for this sync batch.");

        builder.Property(csl => csl.TotalPages)
            .HasComment("Total pages available for this sync batch (from TMDb).");

        builder.Property(csl => csl.Notes)
            .HasComment("Optional notes or error details for debugging.");

        // Ensure uniqueness → one log per (Type, Year, Month)
        builder.HasIndex(csl => new { csl.Type, csl.Year, csl.Month })
            .IsUnique();

        // Audit fields
        builder.Property(csl => csl.Created).HasComment("Date and time when the record was created.");
        builder.Property(csl => csl.CreatedBy).HasComment("User who created the record.");
        builder.Property(csl => csl.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(csl => csl.LastModifiedBy).HasComment("User who last modified the record.");
    }
}

class DailyApiUsageConfiguration : IEntityTypeConfiguration<DailyApiUsage>
{
    public void Configure(EntityTypeBuilder<DailyApiUsage> builder)
    {
        builder.Property(dau => dau.Id)
            .HasComment("Internal primary key for DailyApiUsage.");
        builder.Property(dau => dau.Uuid)
            .HasComment("Unique UUID identifier for DailyApiUsage.");

        builder.Property(dau => dau.Date)
            .HasComment("Date of the API usage tracking (daily basis).");

        builder.Property(dau => dau.ContentType)
            .HasConversion<int>()
            .HasComment("Content type for which requests were made: 1=Movie, 2=Series.");

        builder.Property(dau => dau.RequestCount)
            .HasComment("Number of API requests made for this content type on this date.");

        // Ensure uniqueness → one record per (Date, ContentType)
        builder.HasIndex(dau => new { dau.Date, dau.ContentType })
            .IsUnique();

        // Audit fields
        builder.Property(dau => dau.Created).HasComment("Date and time when the record was created.");
        builder.Property(dau => dau.CreatedBy).HasComment("User who created the record.");
        builder.Property(dau => dau.LastModified).HasComment("Date and time when the record was last modified.");
        builder.Property(dau => dau.LastModifiedBy).HasComment("User who last modified the record.");
    }
}
