namespace FlixHub.Core.Api.Repository;

class FlixHubDbContext(DbContextOptions<FlixHubDbContext> options,
                       IIdGeneratorService idGeneratorService,
                       ICurrentUserService currentUserService) :
    GenericDbContext(options, idGeneratorService, currentUserService)
{
    protected override string Schema => "dbo";

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
}