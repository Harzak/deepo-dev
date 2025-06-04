namespace Deepo.Fetcher.DAL.EF.Models
{
    public partial class Fetcher
    {
        public Fetcher()
        {
            Executions = new HashSet<Execution>();
            Planifications = new HashSet<Planification>();
        }

        public int Fetcher_ID { get; set; }
        public string Fetcher_GUID { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Execution> Executions { get; set; }
        public virtual ICollection<Planification> Planifications { get; set; }
    }
}
