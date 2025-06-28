using Framework.Common.Utils.Result;

namespace Deepo.DAL.Repository.Result;

public class DatabaseOperationResult : ResultBase
{
    public int RowAffected { get; set; }

    public DatabaseOperationResult()
    {

    }

    public DatabaseOperationResult(bool result) : base(result)
    {

    }
}