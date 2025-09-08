namespace FlixHub.Core.Api.Repository;

#region [ interfaces ]

interface ISystemUsersRepository : IGenericRepository<SystemUser, SystemUserDto> { }
interface IContentsRepository : IGenericRepository<Content, ContentDto> { }
interface IGenresRepository : IGenericRepository<Genre, GenreDto> { }
interface IContentGenresRepository : IGenericRepository<ContentGenre, ContentGenreDto> { }
interface IPersonsRepository : IGenericRepository<Person, PersonDto> { }
interface IContentCastsRepository : IGenericRepository<ContentCast, ContentCastDto> { }
interface IContentCrewsRepository : IGenericRepository<ContentCrew, ContentCrewDto> { }

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

#endregion