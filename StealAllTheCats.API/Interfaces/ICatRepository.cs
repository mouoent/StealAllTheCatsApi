using StealAllTheCats.API.Models.Entities;

namespace StealAllTheCats.API.Interfaces;

public interface ICatRepository
{
    Task<List<Cat>> GetCatsAsync();
    Task<List<Cat>> GetCatsByTagAsync(string tag);
    Task<List<Cat>> GetCatsWithoutImagesAsync();
    Task<Cat?> GetCatByIdAsync(int id);
    Task<Cat?> GetCatByIdAsync(string catId);
    Task<HashSet<string>> GetExistingCatIdsAsync();
    Task AddCatsAsync(IEnumerable<Cat> cats);
    Task UpdateCatAsync(Cat cat);
    Task UpdateCatsAsync(IEnumerable<Cat> cats);    
    Task SaveChangesAsync();
}
