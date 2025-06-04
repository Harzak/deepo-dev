namespace Deepo.Fetcher.DAL.EF.Models
{
    public partial class V_ActiveQueueItem
    {
        public int Queue_ID { get; set; }
        public string Fetcher_GUID { get; set; } = null!;
        public DateTime? EnqueueDate { get; set; }
        public DateTime DequeueDate { get; set; }
    }
}
