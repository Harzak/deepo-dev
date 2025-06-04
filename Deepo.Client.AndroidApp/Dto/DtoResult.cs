namespace Deepo.Client.AndroidApp.Dto
{
    [Serializable]
    public class DtoResult<TModel> where TModel : class
    {
        public bool HasContent { get; set; }
        public TModel? Content { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsFailed { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public int ErrorCode { get; set; }
    }
}
