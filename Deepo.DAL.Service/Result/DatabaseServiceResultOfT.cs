using Framework.Common.Utils.Result;

namespace Deepo.DAL.Service.Result;

public class DatabaseServiceResult<T> : OperationResult<T>
{
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
