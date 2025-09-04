namespace Tasheer.Core.Database;

public abstract class GenericDbContext(DbContextOptions options,
                                       IIdGeneratorService idGeneratorService,
                                       ICurrentUserService currentUserService)
    : DbContext(options)
{
    protected abstract string Schema { get; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (!string.IsNullOrWhiteSpace(Schema))
        {
            modelBuilder.HasDefaultSchema(Schema);
        }

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var userId = currentUserService.UserId ?? string.Empty;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Uuid = entry.Entity.Uuid == Guid.Empty ? idGeneratorService.NewID() : entry.Entity.Uuid;
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.Created = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
            }

            // Handle concurrency
            if (entry.Entity is IConcurrencyAware concurrencyAwareEntity)
            {
                concurrencyAwareEntity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
            }
        }

        // Handle concurrency for modified entries
        var updatedConcurrencyAwareEntries =
            ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Modified)
            .OfType<IConcurrencyAware>();

        foreach (var entry in updatedConcurrencyAwareEntries)
            entry.ConcurrencyStamp = Guid.NewGuid().ToString();

        return await base.SaveChangesAsync(cancellationToken);
    }
}