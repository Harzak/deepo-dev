namespace Deepo.Framework.Interfaces;

public interface IResult<T> : IResult
{
    T Content { get; }
}