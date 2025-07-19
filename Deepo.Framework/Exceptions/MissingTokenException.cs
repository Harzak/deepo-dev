namespace Deepo.Framework.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a required authentication token is missing or unavailable.
/// </summary>
public class MissingTokenException : Exception
{
    public MissingTokenException()
    {

    }
    public MissingTokenException(string message) : base(message)
    {

    }

    public MissingTokenException(string message, System.Exception innerException) : base(message, innerException)
    {

    }
}