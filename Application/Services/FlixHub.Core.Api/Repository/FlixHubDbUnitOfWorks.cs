namespace FlixHub.Core.Api.Repository;

interface IFlixHubDbUnitOfWork : IUnitOfWork
{
    ISystemUsersRepository SystemUsersRepository { get; }
    IContentsRepository ContentsRepository { get; }
    IGenresRepository GenresRepository { get; }
    IContentGenresRepository ContentGenresRepository { get; }
    IPersonsRepository PersonsRepository { get; }
    IContentCastsRepository ContentCastsRepository { get; }
    IContentCrewsRepository ContentCrewsRepository { get; }
}

class FlixHubDbUnitOfWork(FlixHubDbContext context) : UnitOfWork(context), IFlixHubDbUnitOfWork
{
    public ISystemUsersRepository SystemUsersRepository => new SystemUsersRepository(context);
    public IContentsRepository ContentsRepository => new ContentsRepository(context);
    public IGenresRepository GenresRepository => new GenresRepository(context);
    public IContentGenresRepository ContentGenresRepository => new ContentGenresRepository(context);
    public IPersonsRepository PersonsRepository => new PersonsRepository(context);
    public IContentCastsRepository ContentCastsRepository => new ContentCastsRepository(context);
    public IContentCrewsRepository ContentCrewsRepository => new ContentCrewsRepository(context);
}