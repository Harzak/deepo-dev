using System;

namespace Deepo.Fetcher.Viewer.Models;

/// <summary>
/// Provides event data for HTTP request log events, containing information about the logged request.
/// </summary>
public class HttpRequestLogEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the HTTP request information that was logged.
    /// </summary>
    public string Request { get; set; }

    public HttpRequestLogEventArgs(string request)
    {
        Request = request;
    }
}