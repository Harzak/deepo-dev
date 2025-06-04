namespace Deepo.DAL.EF.Models
{
    public partial class Provider_Author
    {
        public int Provider_Author_ID { get; set; }
        public int Provider_ID { get; set; }
        public int Author_ID { get; set; }
        public string Provider_Author_Identifier { get; set; } = null!;

        public virtual Author Author { get; set; } = null!;
        public virtual Provider Provider { get; set; } = null!;
    }
}
