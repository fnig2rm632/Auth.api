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
    
    public static class Session
    {
        public static Error IdCannotBeEmpty => Validation("Session ID cannot be empty");
        public static Error RefreshTokenHashCannotBeEmpty => Validation("Refresh token hash cannot be empty");
        public static Error IpAddressCannotBeEmpty => Validation("IpAddress cannot be empty");
        public static Error InvalidExpirationDate => Validation("Expiration date cannot be in the max or min date");
        public static Error InvalidCreationDate => Validation("Creation date cannot be in the max or min date");
        public static Error InvalidRevocationDate => Validation("Revocation date cannot be in the max or min date");
        public static Error ExpiresAtCannotBeInPast => Validation("Expires  at can't be in the past");
        public static Error CreationDateInFuture => Validation("Creation  date can't be in the future");
        public static Error CreationDateAfterExpiration => Validation("Creation date can't be after expiration");
        public static Error RevokedBeforeCreation => Validation("Revoked data cannot be before creation data");
        public static Error RevocationDateInFuture => Validation("Revocation date can't be in the future");
        public static Error AlreadyRevoked => Validation("Already revoked session");
        public static Error Revoked => Validation("Session revoked ");
    }
    
    public static class User
    {
        public static Error IdCannotBeEmpty => Validation("User ID cannot be empty");
        public static Error LoginCannotBeNull => Validation("Login user value cannot be empty");
        public static Error EmailCannotBeNull => Validation("Email user value cannot be empty");
        public static Error PasswordHashCannotBeNull => Validation("Password hash user value cannot be empty");
        public static Error CreatedAtInFuture => Validation("User created at in the future");
        public static Error NewPasswordMustBeDifferent => Conflict("New password must be different");
    }
    
    public static class Code
    {
        public static Error CreatedAtInFuture => Validation("Code created at in the future");
        public static Error ValueCannotBeEmpty => Validation("Code code value cannot be empty");
        public static Error AlreadyRevoked => Validation("Already revoked session");
        public static Error InvalidHashEntry => Validation("Hash Code value cannot be empty");
        public static Error ExpiredOrMissing => NotFound("Verification code not found or expired");
        public static Error InvalidCode => Validation("Invalid verification code");
        public static Error ReasonMismatch => Validation("Code was not requested for this action");
    }
    
    public static class Device
    {   
        public static Error IdCannotBeEmpty => Validation("Device ID cannot be empty");
    }
    
    public static class Platform
    {   
        public static Error IdCannotBeNegative => Validation("Platform ID cannot be negative");
        public static Error NameCannotBeEmpty => Validation("Name cannot be empty");
        public static Error NameTooLong => Validation("Platform name cannot be more than 255 characters");

    }

    public static class Provider
    {
        public static Error IdCannotBeNegative => Validation("Provider ID cannot be negative");
        public static Error NameCannotBeEmpty => Validation("Provider name cannot be empty");
        public static Error NameTooLong => Validation("Provider name cannot be more than 255 characters");

    }

    public static class OAuthToken
    {
        public static Error IdCannotBeEmpty => Validation("ID OAuth token cannot be empty");
        public static Error AccessTokenCannotBeEmpty => Validation("Access token cannot be empty");
        public static Error RefreshTokenCannotBeEmpty => Validation("Refresh token cannot be empty");
        public static Error InvalidExpirationDate => Validation("Expiration date cannot be in the max or min date");
        public static Error ExpiresAtCannotBeInPast => Validation("Expires at can't be in the past");
    }
    
    public static class UserIdentity
    {
        public static Error IdCannotBeEmpty => Validation("User ID cannot be empty");
        public static Error ProviderUserIdCannotBeEmpty => Validation("Provider user ID cannot be empty");
        public static Error ProviderUserIdTooLong => Validation("Provider user ID cannot be more than 255 characters");
        public static Error CreatedAtInFuture => Validation("Created time at in the future");
    }
    
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