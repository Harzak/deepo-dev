namespace Deepo.Fetcher.DAL.EF.Models
{
    public partial class Planning
    {
        public Planning()
        {
            Planifications = new HashSet<Planification>();
        }

        public int Planing_ID { get; set; }
        public int? HourStart { get; set; }
        public int? MinuteStart { get; set; }
        public int? HourEnd { get; set; }
        public int? MinuteEnd { get; set; }

        public virtual ICollection<Planification> Planifications { get; set; }
    }
}
