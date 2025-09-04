namespace FlixHub.Api.Repository;

interface IFlixHubDbUnitOfWork : IUnitOfWork
{
}

class FlixHubDbUnitOfWork(FlixHubDbContext context) : UnitOfWork(context), IFlixHubDbUnitOfWork
{
}