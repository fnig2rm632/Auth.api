using Auth.Domain.Common;

namespace Auth.Domain.Entities;

public sealed class Device : Entity<Guid>
{
    public int? PlatformId { get; private set; }
    
    public Platform? Platform { get; private init; }
    public ICollection<Session> Sessions { get; private set; } = new List<Session>();

    public static Result<Device> Create(Guid id, int platformId)
    {
        var success = ValidateCreationParameters(id, platformId);

        if (!success.IsSuccess)
            return success.Error;
        
        return new Device()
        {
            Id = id,
            PlatformId = platformId
        };
    }
    
    private static Result ValidateCreationParameters(Guid id, int platformId)
    {
        if (id == Guid.Empty)
            return Error.Device.IdCannotBeEmpty;

        if (platformId < 0)
            return Error.Platform.IdCannotBeNegative;
        
        return Result.Success();
    }
}