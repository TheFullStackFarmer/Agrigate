namespace Agrigate.Domain.DTOs;

/// <summary>
/// An agrigate exception thrown when the requested data cannot be found
/// or doesn't exist
/// </summary>
public class DataNotFound : Exception
{
}

/// <summary>
/// An agrigate exception thrown when the request does not pass the required 
/// validation
/// </summary>
public class ValidationError : Exception
{
    public ValidationError(string message) : base(message)
    {
    }
}

/// <summary>
/// An agrigate exception thrown when a request is unauthorized 
/// </summary>
public class Unauthorized : Exception
{    
}