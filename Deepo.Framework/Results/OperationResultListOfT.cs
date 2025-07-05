namespace Deepo.Framework.Results;

[Serializable]
public class OperationResultList<T> : OperationResult<IList<T>>
{
    public override bool HasContent => base.HasContent && base.Content.Any();

    public OperationResultList()
    {
        this.Content = [];
    }

    public new OperationResultList<T> WithSuccess()
    {
        base.WithSuccess();
        return this;
    }

    public new OperationResultList<T> WithFailure()
    {
        base.WithFailure();
        return this;
    }

    public new OperationResultList<T> WithError(string message)
    {
        base.WithError(message);
        return this;
    }

    public new OperationResultList<T> WithValue(IList<T> value)
    {
        this.Content = value;
        return this;
    }
}