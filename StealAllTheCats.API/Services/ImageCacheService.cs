using StealAllTheCats.API.Interfaces;

namespace StealAllTheCats.API.Services;

public class ImageCacheService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpClientFactory _httpClientFactory;

    public ImageCacheService(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory)
    {
        _serviceProvider = serviceProvider;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var repoService = scope.ServiceProvider.GetRequiredService<ICatRepository>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var catsWithoutImages = await repoService.GetCatsWithoutImagesAsync();

            foreach (var cat in catsWithoutImages)
            {
                try
                {
                    var httpClient = _httpClientFactory.CreateClient();
                    var imageBytes = await httpClient.GetByteArrayAsync(cat.ImageUrl);
                    cat.ImageData = imageBytes;
                }
                catch
                {
                    Console.WriteLine($"Failed to cache image for CatId: {cat.CatId}");
                }
            }
            
            await repoService.UpdateCatsAsync(catsWithoutImages);
            await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken); // Run every 15 minutes
        }
    }
}
