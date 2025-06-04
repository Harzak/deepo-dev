using Microsoft.EntityFrameworkCore;

namespace Deepo.Fetcher.DAL.EF.Models
{
    public partial class DEEPOFetcherContext : DbContext
    {
        public DEEPOFetcherContext()
        {
        }

        public DEEPOFetcherContext(DbContextOptions<DEEPOFetcherContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Execution> Executions { get; set; } = null!;
        public virtual DbSet<Fetcher> Fetchers { get; set; } = null!;
        public virtual DbSet<HttpRequest> HttpRequests { get; set; } = null!;
        public virtual DbSet<Planification> Planifications { get; set; } = null!;
        public virtual DbSet<PlanificationType> PlanificationTypes { get; set; } = null!;
        public virtual DbSet<Planning> Plannings { get; set; } = null!;
        public virtual DbSet<V_CompletedFetcher> V_CompletedFetchers { get; set; } = null!;
        public virtual DbSet<V_InProgressFetcher> V_InProgressFetchers { get; set; } = null!;
        public virtual DbSet<V_Plannification_Fetcher> V_Plannification_Fetchers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Execution>(entity =>
            {
                entity.HasKey(e => e.Execution_ID);

                entity.ToTable("Execution");

                entity.Property(e => e.EndTime).HasColumnType("date");

                entity.Property(e => e.StartTime).HasColumnType("date");

                entity.HasOne(d => d.Fetcher)
                    .WithMany(p => p.Executions)
                    .HasForeignKey(d => d.Fetcher_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Execution_Fetchers");
            });

            modelBuilder.Entity<Fetcher>(entity =>
            {
                entity.HasKey(e => e.Fetcher_ID)
                    .HasName("PK_Fetchers");

                entity.ToTable("Fetcher");

                entity.Property(e => e.Fetcher_GUID).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(300);
            });

            modelBuilder.Entity<HttpRequest>(entity =>
            {
                entity.HasKey(e => e.HttpRequest_ID);

                entity.ToTable("HttpRequest");

                entity.Property(e => e.ApplicationName).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("text");

                entity.Property(e => e.DateLogCreation).HasColumnType("datetime");

                entity.Property(e => e.HttpMethod).HasMaxLength(10);

                entity.Property(e => e.HttpResponse).HasMaxLength(50);

                entity.Property(e => e.JobID).HasMaxLength(50);

                entity.Property(e => e.LevelLog)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.RequestUri).HasColumnType("text");

                entity.Property(e => e.ResponseMessage).HasColumnType("text");

                entity.Property(e => e.Token).HasMaxLength(100);

                entity.Property(e => e.UserAgent).HasMaxLength(50);
            });

            modelBuilder.Entity<Planification>(entity =>
            {
                entity.HasKey(e => e.Planification_ID);

                entity.ToTable("Planification");

                entity.HasOne(d => d.Fetcher)
                    .WithMany(p => p.Planifications)
                    .HasForeignKey(d => d.Fetcher_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Planification_Fetcher");

                entity.HasOne(d => d.PlanificationType)
                    .WithMany(p => p.Planifications)
                    .HasForeignKey(d => d.PlanificationType_ID)
                    .HasConstraintName("FK_Planification_PlanificationType");

                entity.HasOne(d => d.Planning)
                    .WithMany(p => p.Planifications)
                    .HasForeignKey(d => d.Planning_ID)
                    .HasConstraintName("FK_Planification_Planning");
            });

            modelBuilder.Entity<PlanificationType>(entity =>
            {
                entity.HasKey(e => e.PlanificationType_ID);

                entity.ToTable("PlanificationType");

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Planning>(entity =>
            {
                entity.HasKey(e => e.Planing_ID);

                entity.ToTable("Planning");
            });

            modelBuilder.Entity<V_CompletedFetcher>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_CompletedFetcher");

                entity.Property(e => e.EndTime).HasColumnType("date");

                entity.Property(e => e.Fetcher_GUID).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(300);

                entity.Property(e => e.StartTime).HasColumnType("date");
            });

            modelBuilder.Entity<V_InProgressFetcher>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_InProgressFetcher");

                entity.Property(e => e.Fetcher_GUID).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(300);

                entity.Property(e => e.StartTime).HasColumnType("date");
            });

            modelBuilder.Entity<V_Plannification_Fetcher>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Plannification_Fetcher");

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.Fetcher_GUID).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(300);

                entity.Property(e => e.PlanificationTypeName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
