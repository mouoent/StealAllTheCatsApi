using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StealAllTheCats.API.Persistence;

public class StealAllTheCatsContextFactory : IDesignTimeDbContextFactory<StealAllTheCatsContext>
{
    public StealAllTheCatsContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) // Use Development settings if available
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<StealAllTheCatsContext>();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

        return new StealAllTheCatsContext(optionsBuilder.Options);
    }
}
