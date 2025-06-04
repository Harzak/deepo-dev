using Microsoft.EntityFrameworkCore;

namespace Deepo.DAL.EF.Models;

public partial class DEEPOContext : DbContext
{
    private string _connString { get; }

    public DEEPOContext(string connString)
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
