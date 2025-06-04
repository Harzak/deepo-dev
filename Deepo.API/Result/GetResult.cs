namespace Deepo.API.Result;

public class GetResult
{
    public bool Success { get; set; }
    public int Count { get; set; }
    public ErrorList Errors { get; }

    public GetResult()
    {
        Errors = [];
    }
}

