namespace Auth.Domain.Common;

public record Error(string Message, ErrorType ErrorType)
{
    public static Error None => new Error(string.Empty, ErrorType.None);
    
    private static Error Validation(string message = "Invalid data format") => new(message, ErrorType.Validation);
    private static Error NotFound(string message = "Resource not found") => new(message, ErrorType.NotFound);
    private static Error BusinessRule(string message = "Broken business rule") => new(message, ErrorType.BusinessRule);  
    private static Error Conflict(string message = "Conflict data") => new(message, ErrorType.Conflict);  
    private static Error Internal(string message = "Something went wrong") => new(message, ErrorType.Internal);
    private static Error Unavailable(string massage = "Service Unavailable") => new(massage, ErrorType.Unavailable);
    private static Error Timeout(string massage = "Gateway Timeout") => new(massage, ErrorType.Timeout);
    
}

public enum ErrorType
{
    None = 000,
    Validation = 400,
    NotFound = 	404,
    BusinessRule = 422,
    Conflict = 409,
    Unavailable = 503,
    Internal = 500,
    Timeout = 504
}