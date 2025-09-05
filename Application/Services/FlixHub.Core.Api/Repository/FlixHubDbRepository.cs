namespace FlixHub.Core.Api.Repository;

#region [ interfaces ]

interface ISystemUsersRepository : IGenericRepository<SystemUser, SystemUserDto> { }

#endregion

#region [ implementation ]

class SystemUsersRepository(FlixHubDbContext context)
   : GenericRepository<SystemUser, SystemUserDto>(context), ISystemUsersRepository
{ }

#endregion