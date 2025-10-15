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
    IContentRatingsRepository ContentRatingsRepository { get; }
    IContentImagesRepository ContentImagesRepository { get; }
    IContentSeasonsRepository ContentSeasonsRepository { get; }
    IEpisodesRepository EpisodesRepository { get; }
    IEpisodeCrewsRepository EpisodeCrewsRepository { get; }
    IWatchlistsRepository WatchlistsRepository { get; }
    IContentSyncLogsRepository ContentSyncLogsRepository { get; }
    IDailyApiUsagesRepository DailyApiUsagesRepository { get; }
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
    public IContentRatingsRepository ContentRatingsRepository => new ContentRatingsRepository(context);
    public IContentImagesRepository ContentImagesRepository => new ContentImagesRepository(context);
    public IContentSeasonsRepository ContentSeasonsRepository => new ContentSeasonsRepository(context);
    public IEpisodesRepository EpisodesRepository => new EpisodesRepository(context);
    public IEpisodeCrewsRepository EpisodeCrewsRepository => new EpisodeCrewsRepository(context);
    public IWatchlistsRepository WatchlistsRepository => new WatchlistsRepository(context);
    public IContentSyncLogsRepository ContentSyncLogsRepository => new ContentSyncLogsRepository(context);
    public IDailyApiUsagesRepository DailyApiUsagesRepository => new DailyApiUsagesRepository(context);
}