using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.ValueObjects;

namespace TchotchomereCore.Infrastructure.Sql.DataContexts;
public class DataContext : DbContext
{
    private readonly IEnumerable<ISaveChangesInterceptor> _auditableEntitySaveChangesInterceptor;
    private readonly ILoggerFactory _loggerFactory;

    public DbSet<ExtractedURL> ExtractedURL { get; set; }
    public DbSet<InitialURL> InitialURL { get; set; }
    public DbSet<Film> Film { get; set; }
    public DbSet<Serie> Serie { get; set; }
    public DbSet<Genre> Genre { get; set; }
    public DbSet<DataDownload> DataDownload { get; set; }

    public DataContext(
        DbContextOptions<DataContext> options,
        IEnumerable<ISaveChangesInterceptor> auditableEntitySaveChangesInterceptor,
        ILoggerFactory loggerFactory)
        : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        _loggerFactory = loggerFactory;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        modelBuilder.Entity<ExtractedURL>()
            .HasIndex(b => b.Url)
            .IsUnique();

        modelBuilder.Entity<ExtractedURL>()
            .HasIndex(b => b.ProcessedDateTime);

        modelBuilder.Entity<ExtractedURL>()
            .HasIndex(b => b.BaseUrl);

        modelBuilder.Entity<Film>().OwnsOne(x => x.ImdbData, sa =>
        {
            sa.Property(x => x.BackdropPicture).HasColumnName(nameof(ImdbData.BackdropPicture));
            sa.Property(x => x.Date).HasColumnName(nameof(ImdbData.Date));
            sa.Property(x => x.IDIMDb).HasColumnName(nameof(ImdbData.IDIMDb));
            sa.Property(x => x.IDTheMovieDB).HasColumnName(nameof(ImdbData.IDTheMovieDB));
            sa.Property(x => x.PosterPicture).HasColumnName(nameof(ImdbData.PosterPicture));
        });

        modelBuilder.Entity<Serie>().OwnsOne(x => x.ImdbData, sa =>
        {
            sa.Property(x => x.BackdropPicture).HasColumnName(nameof(ImdbData.BackdropPicture));
            sa.Property(x => x.Date).HasColumnName(nameof(ImdbData.Date));
            sa.Property(x => x.IDIMDb).HasColumnName(nameof(ImdbData.IDIMDb));
            sa.Property(x => x.IDTheMovieDB).HasColumnName(nameof(ImdbData.IDTheMovieDB));
            sa.Property(x => x.PosterPicture).HasColumnName(nameof(ImdbData.PosterPicture));
        });


        var genresToAdd = Domain.Entities.Genre.MovieGenres
            .ToList();

        modelBuilder.Entity<Genre>().HasData(genresToAdd);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }
}
