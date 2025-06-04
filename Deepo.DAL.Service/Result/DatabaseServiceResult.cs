using Framework.Common.Utils.Result;

namespace Deepo.DAL.Service.Result;

public class DatabaseServiceResult : ResultBase
{
    public int RowAffected { get; set; }

    public DatabaseServiceResult()
    {

    }

    public DatabaseServiceResult(bool result) : base(result)
    {

    }
}