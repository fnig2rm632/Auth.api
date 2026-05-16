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
    
    public static class Email
    {
        public static Error ValueCannotBeEmpty => Validation("Email value cannot be empty");
        public static Error TooLong(string email) => Validation($"Email {email} cannot be so long");
        public static Error InvalidAddress(string email) => Validation($"Invalid {email} email address");
    }
    
    public static class Login
    {
        public static Error ValueCannotBeEmpty => Validation("Login value cannot be empty");
        public static Error ContainsWhitespace => Validation("Login must not contain whitespace characters");  
        public static Error TooShort => Validation("Login cannot be so short");
        public static Error TooLong=> Validation("Login cannot be so long");
        public static Error InvalidCapitalLetters => Validation("Login cannot contain capital letters");
        public static Error InvalidNumbers => Validation("Login cannot contain numbers");
        public static Error ContainsInvalidWords => Validation("Login contains invalid word");
        public static Error InvalidSpecialCharacters(string chars) => Validation($"Login can only contain special characters {chars}");
    }

    public static class LoginPolicy
    {
        public static Error InvalidMinNumber => Validation("MinLength must be more than 0");
        public static Error InvalidNumbersValues => Validation("MaxLength must be bigger than MinLength"); 
    }
    
    public static class Password
    {
        public static Error ValueCannotBeEmpty => Validation("Password value cannot be empty");
        public static Error TooShort(int minLength) => Validation($"Password must be at least {minLength} characters long");
        public static Error TooLong(int maxLength) => Validation($"Password cannot be more than {maxLength} characters long");   
        public static Error ContainsWhitespace => Validation("Password must not contain whitespace characters");   
        public static Error LackOfNumber => Validation("Password must contain digits");
        public static Error LackOfLetter => Validation("Password must contain letters");
        public static Error LackUppercase => Validation("Password must contain uppercase");
        public static Error LackLowercase => Validation("Password must contain lowercase");
        public static Error LackOfSpecialCharacters => Validation("Password must not contain special characters");
    }

    public static class PasswordPolicy
    {
        public static Error NoOneTypeChose => Validation("No one password type chosen in configuration");
        public static Error MinLengthIsSmall => Validation("Min Length cannot be less than 4 characters");
        public static Error MaxLengthIsBig => Validation("Max Length cannot be more than 255 characters");
        public static Error MinCannotBeMoreMax => Validation("Min Length cannot be Be more than Max Length");
        public static Error RequireSimbolIsNullOrEmpty => Validation("Require simbol cannot be Null Or Empty");
    }
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