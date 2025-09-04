namespace Tasheer.Core.Services;

public class IdGeneratorService : IIdGeneratorService
{
    public Guid NewID() => NewId.NextGuid();
}
