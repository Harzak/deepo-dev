namespace Deepo.Fetcher.DAL.EF.Models
{
    public partial class Execution
    {
        public int Execution_ID { get; set; }
        public int Fetcher_ID { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual Fetcher Fetcher { get; set; } = null!;
    }
}
