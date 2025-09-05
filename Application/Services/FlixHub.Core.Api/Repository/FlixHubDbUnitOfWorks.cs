namespace FlixHub.Core.Api.Repository;

interface IFlixHubDbUnitOfWork : IUnitOfWork
{
    ISystemUsersRepository SystemUsersRepository { get; }
}

class FlixHubDbUnitOfWork(FlixHubDbContext context) : UnitOfWork(context), IFlixHubDbUnitOfWork
{
    public ISystemUsersRepository SystemUsersRepository => new SystemUsersRepository(context);
}