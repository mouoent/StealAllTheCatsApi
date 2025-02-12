using StealAllTheCats.API.Models.Shared;

namespace StealAllTheCats.Models.Responses;

public class CatApiResponse
{
    public string Id { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public string Url { get; set; } = string.Empty;
    public List<BreedInfo> Breeds { get; set; } = new();
}
