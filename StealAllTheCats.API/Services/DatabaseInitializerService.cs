using Microsoft.EntityFrameworkCore;
using StealAllTheCats.API.Persistence;

namespace StealAllTheCats.API.Services;

public class DatabaseInitializerService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializerService> _logger;

    public DatabaseInitializerService(IServiceProvider serviceProvider, ILogger<DatabaseInitializerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StealAllTheCatsContext>();

        try
        {
            _logger.LogInformation("Applying pending migrations...");
            await context.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database is up to date.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying database migrations.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
