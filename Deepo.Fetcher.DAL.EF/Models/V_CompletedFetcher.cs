namespace Deepo.Fetcher.DAL.EF.Models
{
    public partial class V_CompletedFetcher
    {
        public string Name { get; set; } = null!;
        public string Fetcher_GUID { get; set; } = null!;
        public DateTime? StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
