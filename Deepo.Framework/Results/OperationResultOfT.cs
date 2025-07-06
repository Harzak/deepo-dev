using Deepo.Framework.Interfaces;

namespace Deepo.Framework.Results;

[Serializable]
public class OperationResult<T> : OperationResult, IResult<T>
{
    public T Content { get; set; }

    public virtual bool HasContent
    {
        get => !EqualityComparer<T>.Default.Equals(this.Content, default);
    }

    public OperationResult() : base()
    {
        Content = default!;
    }

    public OperationResult(T content) : base()
    {
        Content = content;
    }

    public OperationResult(T content, bool result) : base(result)
    {
        Content = content;
    }

    public new OperationResult<T> WithSuccess()
    {
        base.WithSuccess();
        return this;
    }

    public new OperationResult<T> WithFailure()
    {
        base.WithFailure();
        return this;
    }

    public new OperationResult<T> WithError(string message)
    {
        base.WithError(message);
        return this;
    }

    public OperationResult<T> WithValue(T value)
    {
        this.Content = value;
        return this;
    }

    public OperationResult<T> Affect<TDifferent>(OperationResult<TDifferent> operationResult)
    {
        ArgumentNullException.ThrowIfNull(operationResult, nameof(operationResult));
        this.IsSuccess = operationResult.IsSuccess;
        this.ErrorMessage = operationResult.ErrorMessage;
        this.ErrorCode = operationResult.ErrorCode;
        return this;

    }

    public OperationResult ToOperationResult()
    {
        return new OperationResult(this.IsSuccess)
        {
            ErrorMessage = this.ErrorMessage,
            ErrorCode = this.ErrorCode
        };
    }
}
