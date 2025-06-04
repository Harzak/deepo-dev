using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Deepo.DAL.EF.Models;

public partial class DEEPOContext : DbContext
{
    public DEEPOContext()
    {
    }

    public DEEPOContext(DbContextOptions<DEEPOContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<Asset_Release> Asset_Releases { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Author_Release> Author_Releases { get; set; }

    public virtual DbSet<Availability_Release> Availability_Releases { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Execution> Executions { get; set; }

    public virtual DbSet<Fetcher> Fetchers { get; set; }

    public virtual DbSet<Genre_Album> Genre_Albums { get; set; }

    public virtual DbSet<Genre_Movie> Genre_Movies { get; set; }

    public virtual DbSet<Genre_TVShow> Genre_TVShows { get; set; }

    public virtual DbSet<HttpRequest> HttpRequests { get; set; }

    public virtual DbSet<Planification> Planifications { get; set; }

    public virtual DbSet<PlanificationType> PlanificationTypes { get; set; }

    public virtual DbSet<Planning> Plannings { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Provider_Release> Provider_Releases { get; set; }

    public virtual DbSet<Release> Releases { get; set; }

    public virtual DbSet<Release_Album> Release_Albums { get; set; }

    public virtual DbSet<Release_Movie> Release_Movies { get; set; }

    public virtual DbSet<Release_TVShow> Release_TVShows { get; set; }

    public virtual DbSet<Type_Asset> Type_Assets { get; set; }

    public virtual DbSet<Type_Release> Type_Releases { get; set; }

    public virtual DbSet<V_FetcherExtended> V_FetcherExtendeds { get; set; }

    public virtual DbSet<V_FetcherPlannification> V_FetcherPlannifications { get; set; }

    public virtual DbSet<V_FetchersLastExecution> V_FetchersLastExecutions { get; set; }

    public virtual DbSet<V_LastVinylRelease> V_LastVinylReleases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Asset_ID);

            entity.ToTable("Asset");

            entity.Property(e => e.Content_Min_URL).IsUnicode(false);
            entity.Property(e => e.Content_URL).IsUnicode(false);

            entity.HasOne(d => d.Type_Asset).WithMany(p => p.Assets)
                .HasForeignKey(d => d.Type_Asset_ID)
                .HasConstraintName("FK_Asset_Asset_Type");
        });

        modelBuilder.Entity<Asset_Release>(entity =>
        {
            entity.HasKey(e => e.Asset_Release_ID);

            entity.ToTable("Asset_Release");

            entity.HasOne(d => d.Asset).WithMany(p => p.Asset_Releases)
                .HasForeignKey(d => d.Asset_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asset_Release_Asset");

            entity.HasOne(d => d.Release).WithMany(p => p.Asset_Releases)
                .HasForeignKey(d => d.Release_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asset_Release_Release");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Author_ID);

            entity.ToTable("Author");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.Provider_Author_Identifier).HasMaxLength(50);

            entity.HasOne(d => d.Provider).WithMany(p => p.Authors)
                .HasForeignKey(d => d.Provider_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Author_Provider");
        });

        modelBuilder.Entity<Author_Release>(entity =>
        {
            entity.HasKey(e => e.Author_Release_ID);

            entity.ToTable("Author_Release");

            entity.HasOne(d => d.Author).WithMany(p => p.Author_Releases)
                .HasForeignKey(d => d.Author_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Author_Release_Author");

            entity.HasOne(d => d.Release).WithMany(p => p.Author_Releases)
                .HasForeignKey(d => d.Release_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Author_Release_Release");
        });

        modelBuilder.Entity<Availability_Release>(entity =>
        {
            entity.HasKey(e => e.Availability_Release_ID);

            entity.ToTable("Availability_Release");

            entity.Property(e => e.Availability_Date).HasColumnType("datetime");

            entity.HasOne(d => d.Country).WithMany(p => p.Availability_Releases)
                .HasForeignKey(d => d.Country_ID)
                .HasConstraintName("FK_Availability_Release_Country");

            entity.HasOne(d => d.Release).WithMany(p => p.Availability_Releases)
                .HasForeignKey(d => d.Release_ID)
                .HasConstraintName("FK_Availability_Release_Release");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Country_ID);

            entity.ToTable("Country");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Execution>(entity =>
        {
            entity.HasKey(e => e.Execution_ID);

            entity.ToTable("Execution", "fetcher");

            entity.Property(e => e.EndedAt).HasColumnType("datetime");
            entity.Property(e => e.StartedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Fetcher).WithMany(p => p.Executions)
                .HasForeignKey(d => d.Fetcher_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Execution_Fetchers");
        });

        modelBuilder.Entity<Fetcher>(entity =>
        {
            entity.HasKey(e => e.Fetcher_ID).HasName("PK_Fetchers");

            entity.ToTable("Fetcher", "fetcher");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Fetcher_GUID).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(300);
        });

        modelBuilder.Entity<Genre_Album>(entity =>
        {
            entity.HasKey(e => e.Genre_Album_ID);

            entity.ToTable("Genre_Album");

            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Release_Albums).WithMany(p => p.Genre_Albums)
                .UsingEntity<Dictionary<string, object>>(
                    "Genre_Album_Release",
                    r => r.HasOne<Release_Album>().WithMany()
                        .HasForeignKey("Release_Album_ID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Genre_Album_Release_Release_Album"),
                    l => l.HasOne<Genre_Album>().WithMany()
                        .HasForeignKey("Genre_Album_ID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Genre_Album_Release_Genre_Album"),
                    j =>
                    {
                        j.HasKey("Genre_Album_ID", "Release_Album_ID");
                        j.ToTable("Genre_Album_Release");
                    });
        });

        modelBuilder.Entity<Genre_Movie>(entity =>
        {
            entity.HasKey(e => e.Genre_Movie_ID);

            entity.ToTable("Genre_Movie");

            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Genre_TVShow>(entity =>
        {
            entity.HasKey(e => e.Genre_TVShow_ID).HasName("PK_Genre_TVShow_ID");

            entity.ToTable("Genre_TVShow");

            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<HttpRequest>(entity =>
        {
            entity.HasKey(e => e.HttpRequest_ID);

            entity.ToTable("HttpRequest", "fetcher");

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
            entity.Property(e => e.UserAgent).HasMaxLength(50);
        });

        modelBuilder.Entity<Planification>(entity =>
        {
            entity.HasKey(e => e.Planification_ID);

            entity.ToTable("Planification", "fetcher");

            entity.HasOne(d => d.Fetcher).WithMany(p => p.Planifications)
                .HasForeignKey(d => d.Fetcher_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Planification_Fetcher");

            entity.HasOne(d => d.PlanificationType).WithMany(p => p.Planifications)
                .HasForeignKey(d => d.PlanificationType_ID)
                .HasConstraintName("FK_Planification_PlanificationType");

            entity.HasOne(d => d.Planning).WithMany(p => p.Planifications)
                .HasForeignKey(d => d.Planning_ID)
                .HasConstraintName("FK_Planification_Planning");
        });

        modelBuilder.Entity<PlanificationType>(entity =>
        {
            entity.HasKey(e => e.PlanificationType_ID);

            entity.ToTable("PlanificationType", "fetcher");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Planning>(entity =>
        {
            entity.HasKey(e => e.Planing_ID);

            entity.ToTable("Planning", "fetcher");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Provider_ID);

            entity.ToTable("Provider");

            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Provider_Release>(entity =>
        {
            entity.HasKey(e => e.Provider_Release_ID).HasName("PK_Source_Release");

            entity.ToTable("Provider_Release");

            entity.Property(e => e.Provider_Release_Identifier).HasMaxLength(400);

            entity.HasOne(d => d.Provider).WithMany(p => p.Provider_Releases)
                .HasForeignKey(d => d.Provider_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Provider_Release_Provider");

            entity.HasOne(d => d.Release).WithMany(p => p.Provider_Releases)
                .HasForeignKey(d => d.Release_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Provider_Release_Release");
        });

        modelBuilder.Entity<Release>(entity =>
        {
            entity.HasKey(e => e.Release_ID);

            entity.ToTable("Release");

            entity.Property(e => e.Creation_Date).HasColumnType("datetime");
            entity.Property(e => e.Creation_User).HasMaxLength(20);
            entity.Property(e => e.GUID).HasMaxLength(68);
            entity.Property(e => e.Modification_Date).HasColumnType("datetime");
            entity.Property(e => e.Modification_User).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(500);

            entity.HasOne(d => d.Type_Release).WithMany(p => p.Releases)
                .HasForeignKey(d => d.Type_Release_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Release_Release_Type");
        });

        modelBuilder.Entity<Release_Album>(entity =>
        {
            entity.HasKey(e => e.Release_Album_ID);

            entity.ToTable("Release_Album");

            entity.Property(e => e.Country).HasMaxLength(2000);
            entity.Property(e => e.Duration).HasMaxLength(50);
            entity.Property(e => e.Label).HasMaxLength(300);

            entity.HasOne(d => d.Release).WithMany(p => p.Release_Albums)
                .HasForeignKey(d => d.Release_ID)
                .HasConstraintName("FK_Release_Album_Release");
        });

        modelBuilder.Entity<Release_Movie>(entity =>
        {
            entity.HasKey(e => e.Release_Movie_ID);

            entity.ToTable("Release_Movie");

            entity.HasOne(d => d.Genre_Movie).WithMany(p => p.Release_Movies)
                .HasForeignKey(d => d.Genre_Movie_ID)
                .HasConstraintName("FK_Release_Movie_Genre_Movie");

            entity.HasOne(d => d.Release).WithMany(p => p.Release_Movies)
                .HasForeignKey(d => d.Release_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Release_Movie_Release");
        });

        modelBuilder.Entity<Release_TVShow>(entity =>
        {
            entity.HasKey(e => e.Release_TVShow_ID).HasName("PK_Release_Serie");

            entity.ToTable("Release_TVShow");

            entity.Property(e => e.Season).HasMaxLength(50);

            entity.HasOne(d => d.Genre_TVShow).WithMany(p => p.Release_TVShows)
                .HasForeignKey(d => d.Genre_TVShow_ID)
                .HasConstraintName("FK_Release_TVShow_Genre_TVShow");

            entity.HasOne(d => d.Release).WithMany(p => p.Release_TVShows)
                .HasForeignKey(d => d.Release_ID)
                .HasConstraintName("FK_Release_Serie_Release");
        });

        modelBuilder.Entity<Type_Asset>(entity =>
        {
            entity.HasKey(e => e.Type_Asset_ID).HasName("PK_Asset_Type");

            entity.ToTable("Type_Asset");

            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Type_Release>(entity =>
        {
            entity.HasKey(e => e.Type_Release_ID).HasName("PK_Release-Type");

            entity.ToTable("Type_Release");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<V_FetcherExtended>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_FetcherExtended", "fetcher");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.FetcherName).HasMaxLength(300);
            entity.Property(e => e.Fetcher_GUID).HasMaxLength(50);
            entity.Property(e => e.LastEnd).HasColumnType("datetime");
            entity.Property(e => e.LastStart).HasColumnType("datetime");
            entity.Property(e => e.PlanificationTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<V_FetcherPlannification>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_FetcherPlannification", "fetcher");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Fetcher_GUID).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.PlanificationCode).HasMaxLength(10);
            entity.Property(e => e.PlanificationTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<V_FetchersLastExecution>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_FetchersLastExecution", "fetcher");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.EndedAt).HasColumnType("datetime");
            entity.Property(e => e.Fetcher_GUID).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.StartedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<V_LastVinylRelease>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_LastVinylRelease");

            entity.Property(e => e.AlbumName).HasMaxLength(500);
            entity.Property(e => e.ArtistsNames).HasMaxLength(4000);
            entity.Property(e => e.Cover_URL).IsUnicode(false);
            entity.Property(e => e.Creation_Date).HasColumnType("datetime");
            entity.Property(e => e.ReleasGUID).HasMaxLength(68);
            entity.Property(e => e.Thumb_URL).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
