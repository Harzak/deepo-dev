namespace Deepo.Framework.Results;

[Serializable]
public class OperationResult : ResultBase
{
    public OperationResult() : base()
    {

    }

    public OperationResult(bool success) : base(success)
    {

    }

    public OperationResult WithSuccess()
    {
        base.IsSuccess = true;
        return this;
    }

    public OperationResult WithFailure()
    {
        base.IsSuccess = false;
        return this;
    }

    public OperationResult WithError(string message)
    {
        base.ErrorMessage = message;
        return this.WithFailure();
    }

    public static OperationResult Success()
    {
        return new OperationResult(true);
    }

    public static OperationResult Failure()
    {
        return new OperationResult(false);
    }

    public static OperationResult Failure(string message)
    {
        return new OperationResult(false).WithError(message);
    }
}
