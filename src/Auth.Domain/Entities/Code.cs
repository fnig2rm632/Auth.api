using Auth.Domain.Common;
using StackExchange.Redis;

namespace Auth.Domain.Entities;

public class Code : Entity
{
    public string Value { get; private init; } = null!;
    public bool IsConfirm { get; private set; }
    public DateTime CreatedAt { get; private init; }
    
    private const string ValueField = "Value";
    private const string IsConfirmField = "IsConfirm";
    private const string CreatedAtField = "CreatedAt";

    public static Result<Code> Create(string value, DateTime createdAt)
    {
        var success = ValidateCreationParameters(
            value, createdAt);

        if (!success.IsSuccess)
            return success.Error;

        return new Code()
        {
            Value = value,
            IsConfirm = false,
            CreatedAt = createdAt
        };
    }

    public static Result<Code> Create(HashEntry[]? hashes)
    {
        if (hashes == null || hashes.Length == 0)
            return Error.Code.InvalidHashEntry;

        string? value = null;
        bool? isConfirm = null;
        DateTime? createdAt = null;
        
        foreach (var entry in hashes)
        {
            var fieldName = entry.Name.ToString();
            var fieldValue = entry.Value;

            switch (fieldName)
            {
                case ValueField:
                    value = fieldValue.ToString();
                    break;
                case IsConfirmField:
                    if (bool.TryParse(fieldValue.ToString(), out var parsedIsConfirm))
                        isConfirm = parsedIsConfirm;
                    break;
                case CreatedAtField:
                    if (DateTime.TryParse(fieldValue.ToString(), out var parsedCreatedAt))
                        createdAt = parsedCreatedAt;
                    break;
            }
        }

        return new Code()
        {
            Value = value!,
            IsConfirm = isConfirm ?? false,
            CreatedAt = createdAt!.Value
        };
    }

    public Result<HashEntry[]> GetHashEntry()
    {
        return new []
        {
            new HashEntry(ValueField, Value),
            new HashEntry(IsConfirmField, IsConfirm.ToString()),
            new HashEntry(CreatedAtField, CreatedAt.ToString("O"))
        };
    }

    public Result Confirm()
    {
        if (IsConfirm)
            return Error.Code.AlreadyRevoked;

        IsConfirm = true;
        
        return Result.Success();
    }
    
    
    private static Result ValidateCreationParameters(string value, DateTime createdAt)
    {
        if (string.IsNullOrEmpty(value))
            return Error.Code.ValueCannotBeEmpty;

        if (createdAt > DateTime.UtcNow.AddMinutes(1))
            return Error.Code.CreatedAtInFuture;
        
        return Result.Success();
    }
}