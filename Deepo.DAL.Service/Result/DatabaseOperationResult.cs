using Framework.Common.Utils.Result;

namespace Deepo.DAL.Service.Result;

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