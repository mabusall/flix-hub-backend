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

        builder.Property(p => p.KeycloakUserId)
            .HasComment("Unique identifier for the user in the Keycloak system.");

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

        builder.Property(p => p.TraktId)
            .HasComment("Trakt slug/ID.");

        // Type (Movie/TV)
        builder.Property(p => p.Type)
            .HasConversion<string>()
            .HasComment("Content type: Movie or TV.");

        // Titles & descriptions
        builder.Property(p => p.Title)
            .HasComment("Display title of the content.");

        builder.Property(p => p.OriginalTitle)
            .HasComment("Original title or name in original language.");

        builder.Property(p => p.Overview)
            .HasComment("Summary / description.");

        builder.Property(p => p.OriginalLanguage)
            .HasMaxLength(10)
            .HasComment("ISO 639-1 language code.");

        // Dates
        builder.Property(p => p.ReleaseDate)
            .HasComment("Movie: release_date / TV: first_air_date.");

        // Status
        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasComment("Release status: Released, Ended, Returning Series.");

        // Country
        builder.Property(p => p.Country)
            .HasMaxLength(10)
            .HasComment("ISO 3166-1 country code.");

        // Runtime
        builder.Property(p => p.Runtime)
            .HasComment("Runtime in minutes (movie or avg episode runtime).");

        // Popularity & votes
        builder.Property(p => p.PopularityTmdb)
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
        builder.HasIndex(p => p.Type);
        builder.HasIndex(p => p.ReleaseDate);
        builder.HasIndex(p => p.PopularityTmdb);
    }
}

class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasIndex(g => g.TmdbId).IsUnique();

        // Primary keys & identifiers
        builder.Property(p => p.Id)
            .HasComment("Internal primary key for Genre.");
        builder.Property(p => p.Uuid)
            .HasComment("Unique UUID identifier for Genre.");
        builder.Property(g => g.TmdbId)
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
            .HasMaxLength(150)
            .HasColumnType("varchar(150)")
            .HasComment("Full name of the person.");

        builder.Property(p => p.Gender)
            .HasConversion<int>() // store as int (0,1,2,3)
            .HasComment("Gender: 0=Unknown, 1=Female, 2=Male, 3=NonBinary.");

        builder.Property(p => p.BirthDate)
            .HasComment("Birth date of the person.");
        builder.Property(p => p.DeathDate)
            .HasComment("Death date of the person.");

        builder.Property(p => p.KnownForDepartment)
            .HasComment("Department this person is best known for (Acting, Directing, Writing).");
        
        builder.Property(p => p.Biography)
            .HasComment("Person Biography.");

        builder.Property(p => p.ProfilePath)
            .HasComment("Profile image path from TMDb.");

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
        builder.HasKey(cc => new { cc.ContentId, cc.PersonId });

        builder.Property(cc => cc.ContentId)
            .HasComment("Foreign key to Content (movie or TV).");
        builder.Property(cc => cc.PersonId)
            .HasComment("Foreign key to Person.");
        builder.Property(cc => cc.Character)
            .HasMaxLength(150)
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
        builder.HasKey(cc => new { cc.ContentId, cc.PersonId });

        builder.Property(cc => cc.ContentId)
            .HasComment("Foreign key to Content (movie or TV).");
        builder.Property(cc => cc.PersonId)
            .HasComment("Foreign key to Person.");
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
