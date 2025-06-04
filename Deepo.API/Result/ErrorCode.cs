using System.ComponentModel;

namespace Deepo.API.Result;

public enum ErrorCode
{
    [Description("Invalid query")]
    InvalidQuery = 100,

    [Description("Invalid market identifier")]
    InvalideMarket = 200,

    None = 0
}

