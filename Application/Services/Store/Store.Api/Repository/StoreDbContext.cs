namespace Store.Api.Repository;

class StoreDbContext(DbContextOptions<StoreDbContext> options,
                     IIdGeneratorService idGeneratorService,
                     ICurrentUserService currentUserService) :
    GenericDbContext(options, idGeneratorService, currentUserService)
{
    protected override string Schema => "dbo";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //var foriegnKeys = modelBuilder
        //    .Model
        //    .GetEntityTypes()
        //    .SelectMany(e => e.GetForeignKeys());

        //foreach (var relationship in foriegnKeys)
        //    relationship.DeleteBehavior = DeleteBehavior.NoAction;

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
}