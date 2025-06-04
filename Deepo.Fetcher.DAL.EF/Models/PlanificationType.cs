namespace Deepo.Fetcher.DAL.EF.Models
{
    public partial class PlanificationType
    {
        public PlanificationType()
        {
            Planifications = new HashSet<Planification>();
        }

        public int PlanificationType_ID { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Planification> Planifications { get; set; }
    }
}
