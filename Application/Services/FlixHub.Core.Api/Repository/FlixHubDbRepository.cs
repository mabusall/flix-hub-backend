namespace FlixHub.Core.Api.Repository;

#region [ interfaces ]

interface ISystemUsersRepository : IGenericRepository<SystemUser, SystemUserDto> { }
interface IContentsRepository : IGenericRepository<Content, ContentDto> { }
interface IGenresRepository : IGenericRepository<Genre, GenreDto> { }
interface IContentGenresRepository : IGenericRepository<ContentGenre, ContentGenreDto> { }
interface IPersonsRepository : IGenericRepository<Person, PersonDto> { }
interface IContentCastsRepository : IGenericRepository<ContentCast, ContentCastDto> { }
interface IContentCrewsRepository : IGenericRepository<ContentCrew, ContentCrewDto> { }
interface IContentRatingsRepository : IGenericRepository<ContentRating, ContentRatingDto> { }
interface IContentImagesRepository : IGenericRepository<ContentImage, ContentImageDto> { }
interface IContentSeasonsRepository : IGenericRepository<ContentSeason, ContentSeasonDto> { }
interface IEpisodesRepository : IGenericRepository<Episode, EpisodeDto> { }
interface IEpisodeCrewsRepository : IGenericRepository<EpisodeCrew, EpisodeCrewDto> { }
interface IWatchlistsRepository : IGenericRepository<Watchlist, WatchlistDto> { }
interface IContentSyncLogsRepository : IGenericRepository<ContentSyncLog, ContentSyncLogDto> { }
interface IDailyApiUsagesRepository : IGenericRepository<DailyApiUsage, DailyApiUsageDto> { }

#endregion

#region [ implementation ]

class SystemUsersRepository(FlixHubDbContext context)
   : GenericRepository<SystemUser, SystemUserDto>(context), ISystemUsersRepository
{ }

class ContentsRepository(FlixHubDbContext context)
   : GenericRepository<Content, ContentDto>(context), IContentsRepository
{ }

class GenresRepository(FlixHubDbContext context)
   : GenericRepository<Genre, GenreDto>(context), IGenresRepository
{ }

class ContentGenresRepository(FlixHubDbContext context)
   : GenericRepository<ContentGenre, ContentGenreDto>(context), IContentGenresRepository
{ }

class PersonsRepository(FlixHubDbContext context)
   : GenericRepository<Person, PersonDto>(context), IPersonsRepository
{ }

class ContentCastsRepository(FlixHubDbContext context)
   : GenericRepository<ContentCast, ContentCastDto>(context), IContentCastsRepository
{ }

class ContentCrewsRepository(FlixHubDbContext context)
   : GenericRepository<ContentCrew, ContentCrewDto>(context), IContentCrewsRepository
{ }

class ContentRatingsRepository(FlixHubDbContext context)
   : GenericRepository<ContentRating, ContentRatingDto>(context), IContentRatingsRepository
{ }

class ContentImagesRepository(FlixHubDbContext context)
   : GenericRepository<ContentImage, ContentImageDto>(context), IContentImagesRepository
{ }

class ContentSeasonsRepository(FlixHubDbContext context)
   : GenericRepository<ContentSeason, ContentSeasonDto>(context), IContentSeasonsRepository
{ }

class EpisodesRepository(FlixHubDbContext context)
   : GenericRepository<Episode, EpisodeDto>(context), IEpisodesRepository
{ }

class EpisodeCrewsRepository(FlixHubDbContext context)
   : GenericRepository<EpisodeCrew, EpisodeCrewDto>(context), IEpisodeCrewsRepository
{ }

class WatchlistsRepository(FlixHubDbContext context)
   : GenericRepository<Watchlist, WatchlistDto>(context), IWatchlistsRepository
{ }

class ContentSyncLogsRepository(FlixHubDbContext context)
   : GenericRepository<ContentSyncLog, ContentSyncLogDto>(context), IContentSyncLogsRepository
{ }

class DailyApiUsagesRepository(FlixHubDbContext context)
   : GenericRepository<DailyApiUsage, DailyApiUsageDto>(context), IDailyApiUsagesRepository
{ }

#endregion