namespace FlixHub.Core.Api.Repository;

class FlixHubDbContext(DbContextOptions<FlixHubDbContext> options,
                       IIdGeneratorService idGeneratorService,
                       ICurrentUserService currentUserService) :
    GenericDbContext(options, idGeneratorService, currentUserService)
{
    protected override string Schema => "public";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var foriegnKeys = modelBuilder
            .Model
            .GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys());

        foreach (var relationship in foriegnKeys)
            relationship.DeleteBehavior = DeleteBehavior.Cascade;

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<SystemUser> SystemUsers => Set<SystemUser>();
    public DbSet<Content> Contents => Set<Content>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<ContentGenre> ContentGenres => Set<ContentGenre>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<ContentCast> ContentCasts => Set<ContentCast>();
    public DbSet<ContentCrew> ContentCrews => Set<ContentCrew>();
    public DbSet<ContentRating> ContentRatings => Set<ContentRating>();
    public DbSet<ContentImage> ContentImages => Set<ContentImage>();
    public DbSet<ContentSeason> ContentSeasons => Set<ContentSeason>();
    public DbSet<Episode> Episodes => Set<Episode>();
    public DbSet<EpisodeCrew> EpisodeCrews => Set<EpisodeCrew>();
    public DbSet<Watchlist> Watchlists => Set<Watchlist>();
    public DbSet<ContentSyncLog> ContentSyncLogs => Set<ContentSyncLog>();
    public DbSet<DailyApiUsage> DailyApiUsages => Set<DailyApiUsage>();
}