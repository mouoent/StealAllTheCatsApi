using StealAllTheCats.API.Interfaces;
using StealAllTheCats.API.Models.Entities;
using StealAllTheCats.Models.Responses;

namespace StealAllTheCats.API.Services;

public class CatApiService : ICatApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;
    private readonly string _apiKey;

    public CatApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiUrl = configuration["CatApiConfig:GetCatsEndpoint"] ?? "";
        _apiKey = configuration["CatApiConfig:Key"] ?? "";
    }

    public async Task<List<Cat>> FetchCatImagesAsync(bool includeTags)
    {
        int requiredCats = 25; // Amount of cats to fetch
        int includeTagsInt = Convert.ToInt32(includeTags);
        string apiUrl = $"{_apiUrl}?limit={requiredCats}&has_breeds={includeTagsInt}&api_key={_apiKey}";

        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return new List<Cat>();
        }

        // Map response to a list of Cat objects and return the list
        var responseData = await response.Content.ReadFromJsonAsync<List<CatApiResponse>>();
        return responseData?.Select(cat => new Cat
        {
            CatId = cat.Id,
            Width = cat.Width,
            Height = cat.Height,
            ImageUrl = cat.Url,
            Tags = cat.Breeds.FirstOrDefault()?.Temperament?.Split(',')
                      .Select(t => new Tag { Name = t.Trim() })
                      .ToList() ?? new List<Tag>()
        }).ToList() ?? new List<Cat>();
    }

    public async Task<byte[]?> DownloadImageAsync(string imageUrl)
    {
        try
        {
            return await _httpClient.GetByteArrayAsync(imageUrl);
        }
        catch
        {
            Console.WriteLine($"Failed to download image: {imageUrl}");
            return null;
        }
    }

}