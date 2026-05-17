using Auth.Domain.Common;

namespace Auth.Domain.Entities;

public class Platform : Entity<int>
{
    public string Name { get; private set; } = string.Empty;
    
    public ICollection<Device> Devices { get; private set; } = new List<Device>();
    
    public static Result<Platform> Create(string name)
    {
        var success = ValidateCreationParameters(name);

        if (!success.IsSuccess)
            return success.Error;
        
        return new Platform
        {
            Name = name
        };
    }

    private static Result ValidateCreationParameters(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Platform.NameCannotBeEmpty;

        if (name.Length > 255)
            return Error.Platform.NameTooLong;
        
        return Result.Success();
    }
}