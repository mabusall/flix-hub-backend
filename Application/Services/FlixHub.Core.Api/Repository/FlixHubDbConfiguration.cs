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
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("varchar(150)")
            .HasComment("Username of the system user.")
            .HasConversion<DbEncryptionProvider>();

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("varchar(150)")
            .HasComment("Email address of the system user.")
            .HasConversion<DbEncryptionProvider>();

        builder.Property(p => p.KeycloakUserId)
            .HasComment("Unique identifier for the user in the Keycloak system.");

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("varchar(150)")
            .HasComment("First name of the system user.")
            .HasConversion<DbEncryptionProvider>();

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("varchar(150)")
            .HasComment("Last name of the system user.")
            .HasConversion<DbEncryptionProvider>();

        builder.Property(p => p.EmailVerificationCode)
            .HasMaxLength(200)
            .HasColumnType("varchar(200)")
            .HasComment("Email verification code for user.");

        builder.Property(p => p.IsActive)
            .HasComment("Indicates whether the user is active.");

        builder.Property(p => p.IsVerified)
            .HasComment("Indicates whether the user is verified.");

        builder.Property(p => p.Preferences)
            .HasColumnType("jsonb")
            .HasComment("User preferences stored as JSON.")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<UserPreferencesDto>(v, (JsonSerializerOptions)null!)!);

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
