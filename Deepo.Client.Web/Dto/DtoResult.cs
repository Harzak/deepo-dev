namespace Deepo.Client.Web.Dto;

[Serializable]
public class DtoResult<TModel>
{
    public bool HasContent { get; set; }
    public TModel? Content { get; set; }
    public bool IsSuccess { get; set; }
    public bool IsFailed { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string ErrorCode { get; set; } = string.Empty;
}
