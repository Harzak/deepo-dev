using Microsoft.EntityFrameworkCore;

namespace Deepo.Fetcher.DAL.EF.Models
{
    public partial class DEEPOFetcherContext : DbContext
    {
        private string _connString { get; }

        public DEEPOFetcherContext(string connString)
        {
            _connString = connString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connString);
            }
        }
    }
}
