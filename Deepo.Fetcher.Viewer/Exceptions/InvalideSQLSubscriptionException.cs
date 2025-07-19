using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Viewer.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an invalid SQL subscription operation occurs.
/// </summary>
public class InvalideSQLSubscriptionException : Exception
{
    public InvalideSQLSubscriptionException()
    {
    }

    public InvalideSQLSubscriptionException(string message) : base(message)
    {
    }

    public InvalideSQLSubscriptionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
