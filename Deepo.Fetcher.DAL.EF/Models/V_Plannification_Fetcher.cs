namespace Deepo.Fetcher.DAL.EF.Models
{
    public partial class V_Plannification_Fetcher
    {
        public string Name { get; set; } = null!;
        public string Fetcher_GUID { get; set; } = null!;
        public string? PlanificationTypeName { get; set; }
        public string? Code { get; set; }
        public int? HourStart { get; set; }
        public int? MinuteStart { get; set; }
        public int? HourEnd { get; set; }
        public int? MinuteEnd { get; set; }
    }
}
