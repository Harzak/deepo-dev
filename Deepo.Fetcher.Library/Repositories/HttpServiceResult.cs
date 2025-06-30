using Framework.Common.Utils.Result;
using System.Net;

namespace Deepo.Fetcher.Library.Repositories;

public class HttpServiceResult<T> : OperationResult<T>
{
    public HttpStatusCode StatusCode { get; set; }

    public HttpServiceResult() { }

    public HttpServiceResult(T content) : base(content) { }

    public HttpServiceResult(T content, bool result) : base(content, result) { }

    public new HttpServiceResult<T> WithSuccess()
    {
        base.WithSuccess();
        return this;
    }

    public new HttpServiceResult<T> WithFailure()
    {
        base.WithFailure();
        return this;
    }

    public new HttpServiceResult<T> WithError(string message)
    {
        base.WithError(message);
        return this;
    }

    public new HttpServiceResult<T> WithValue(T value)
    {
        this.Content = value;
        return this;
    }
}
