namespace Deepo.Fetcher.DAL.EF.Models
{
    public partial class Queue
    {
        public int Queue_ID { get; set; }
        public int Fetcher_ID { get; set; }
        public DateTime? EnqueueDate { get; set; }
        public DateTime DequeueDate { get; set; }

        public virtual Fetcher Fetcher { get; set; } = null!;
    }
}
