using StealAllTheCats.API.Interfaces;

namespace StealAllTheCats.API.Endpoints;

public static class CatEndpoints
{
    public static void MapCatEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/cats/fetch", async (ICatService catService) =>
        {
            await catService.FetchAndStoreUniqueCatsAsync();            
            return Results.Ok("Fetched and stored 25 cat images.");
        });

        app.MapGet("/api/cats/{id}", async (ICatService catService, string catId) =>
        {
            var cat = await catService.GetCatByCatIdAsync(catId);
            return cat != null ? Results.Ok(cat) : Results.NotFound();
        });

        app.MapGet("/api/cats", async (ICatService catService, int page = 1, int pageSize = 10) =>
        {
            var cats = await catService.GetAllCatsPaginatedAsync(page, pageSize);
            return cats != null ? Results.Ok(cats) : Results.NotFound();
        });

        app.MapGet("/api/cats-by-tag", async (ICatService catService, string tag, int page = 1, int pageSize = 10) =>
        {
            var cats = await catService.GetAllCatsPaginatedAsync(tag, page, pageSize);
            return cats != null ? Results.Ok(cats) : Results.NotFound();
        });

        app.MapPost("/api/cats/cache-images", async (ICatService catService) =>
        {
            await catService.CacheCatImagesAsync();            
            return Results.Ok("Images cached successfully.");
        });

        app.MapGet("/api/cats/{catId}/image", async (ICatService catService, string catId) =>
        {
            var imageData = await catService.GetCatImageAsync(catId);            
            if (imageData != null)
            {
                return Results.File(imageData, "image/jpeg");
            }

            // If image data does not exist, return the url
            var cat = await catService.GetCatByCatIdAsync(catId);
            return cat != null ? Results.Redirect(cat.ImageUrl) : Results.NotFound();
        });

    }
}
