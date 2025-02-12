using Microsoft.EntityFrameworkCore;
using StealAllTheCats.API.Models.Entities;

namespace StealAllTheCats.API.Persistence;

public class StealAllTheCatsContext : DbContext
{
    public StealAllTheCatsContext(DbContextOptions<StealAllTheCatsContext> options) : base(options) { }

    public DbSet<Cat> Cats { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cat>()
            .HasIndex(c => c.CatId)
            .IsUnique(); // Every cat fetched needs to be unique
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
}
