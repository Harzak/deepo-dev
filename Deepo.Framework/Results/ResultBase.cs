using Deepo.Framework.Interfaces;

namespace Deepo.Framework.Results;

public abstract class ResultBase : IResult
{
    private bool _isSuccess;
    private bool _isFailed;

    public bool IsSuccess
    {
        get => _isSuccess;
        set
        {
            _isSuccess = value;
            _isFailed = !value;
        }
    }

    public bool IsFailed
    {
        get => _isFailed;
        set
        {
            _isFailed = value;
            _isSuccess = !value;
        }
    }

    public string ErrorMessage { get; set; }

    public string ErrorCode { get; set; }

    protected ResultBase(bool success)
    {
        IsSuccess = success;
        this.ErrorMessage = string.Empty;
        this.ErrorCode = string.Empty;
    }

    protected ResultBase() : this(false)
    {

    }

    public IResult Affect(IResult result)
    {
        if (result != null && IsSuccess)
        {
            IsSuccess = result.IsSuccess;
        }
        return this;
    }

    public IResult Affect(Func<IResult> result)
    {
        if (result != null)
        {
            return Affect(result());
        }
        return this;
    }

    public async Task<IResult> Affect(Func<Task<IResult>> result)
    {
        if (result != null)
        {
            return Affect(await result().ConfigureAwait(false));
        }
        return this;
    }
}
