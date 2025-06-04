namespace Deepo.Mediator.Query;

public class GetAllVinylReleasesWithPaginationQuery : GetAllVinylReleasesQuery
{
    public int Offset { get; set; }
    public int Limit { get; set; }

    public GetAllVinylReleasesWithPaginationQuery(string market) : base(market)
    {

    }
}