using System;

namespace Deepo.Fetcher.Viewer.Models;

public class HttpRequestLogEventArgs : EventArgs
{
    public string Request { get; set; }

    public HttpRequestLogEventArgs(string request)
    {
        Request = request;
    }
}