using Deepo.Framework.Results;

namespace Deepo.DAL.Repository.Result;

/// <summary>
/// Represents the result of a database service operation that returns content of type T along with success status and affected row count.
/// </summary>
/// <typeparam name="T">The type of content returned by the database operation.</typeparam>
public class DatabaseServiceResult<T> : OperationResult<T>
{
    /// <summary>
    /// Gets or sets the number of database rows affected by the operation.
    /// </summary>
    public int RowAffected { get; set; }

    public DatabaseServiceResult()
    {

    }

    public DatabaseServiceResult(T content) : base(content)
    {

    }

    public DatabaseServiceResult(T content, bool result) : base(content, result)
    {

    }
}