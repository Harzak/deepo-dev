using Deepo.Framework.Results;

namespace Deepo.DAL.Repository.Result;

/// <summary>
/// Represents the result of a database operation with success status and affected row count.
/// </summary>
public class DatabaseOperationResult : ResultBase
{
    /// <summary>
    /// Gets or sets the number of database rows affected by the operation.
    /// </summary>
    public int RowAffected { get; set; }

    public DatabaseOperationResult()
    {

    }

    public DatabaseOperationResult(bool result) : base(result)
    {

    }
}