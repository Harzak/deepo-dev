using Deepo.Framework.Results;
using System.Net;

namespace Deepo.Fetcher.Library.Utils;

/// <summary>
/// Represents the result of an HTTP service operation with additional HTTP status code information.
/// Extends <see cref="OperationResult{T}"/> to include HTTP-specific metadata.
/// </summary>
/// <typeparam name="T">The type of content returned by the HTTP operation.</typeparam>
public class HttpServiceResult<T> : OperationResult<T>
{
    /// <summary>
    /// Gets or sets the HTTP status code returned by the service operation.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    public HttpServiceResult()
    {

    }

    public HttpServiceResult(T content) : base(content)
    {

    }

    public HttpServiceResult(T content, bool result) : base(content, result)
    {

    }

    /// <summary>
    /// Marks the HTTP service result as successful and returns the current instance for method chaining.
    /// </summary>
    /// <returns>The current <see cref="HttpServiceResult{T}"/> instance marked as successful.</returns>
    public new HttpServiceResult<T> WithSuccess()
    {
        base.WithSuccess();
        return this;
    }

    /// <summary>
    /// Marks the HTTP service result as failed and returns the current instance for method chaining.
    /// </summary>
    /// <returns>The current <see cref="HttpServiceResult{T}"/> instance marked as failed.</returns>
    public new HttpServiceResult<T> WithFailure()
    {
        base.WithFailure();
        return this;
    }

    /// <summary>
    /// Marks the HTTP service result as failed with an error message and returns the current instance for method chaining.
    /// </summary>
    /// <param name="message">The error message describing the failure.</param>
    /// <returns>The current <see cref="HttpServiceResult{T}"/> instance marked as failed with the specified error message.</returns>
    public new HttpServiceResult<T> WithError(string message)
    {
        base.WithError(message);
        return this;
    }

    /// <summary>
    /// Sets the content value for the HTTP service result and returns the current instance for method chaining.
    /// </summary>
    /// <param name="value">The content value to set.</param>
    /// <returns>The current <see cref="HttpServiceResult{T}"/> instance with the specified content value.</returns>
    public new HttpServiceResult<T> WithValue(T value)
    {
        Content = value;
        return this;
    }
}
