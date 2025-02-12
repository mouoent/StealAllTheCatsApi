using StealAllTheCats.API.Models.Entities;

namespace StealAllTheCats.API.Interfaces
{
    public interface ICatApiService
    {
        Task<byte[]?> DownloadImageAsync(string imageUrl);
        Task<List<Cat>> FetchCatImagesAsync(bool includeTags);
    }
}