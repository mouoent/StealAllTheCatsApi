using StealAllTheCats.API.Interfaces;
using StealAllTheCats.API.Models.DTOs;
using StealAllTheCats.API.Models.Entities;
using StealAllTheCats.API.Models.Responses;

namespace StealAllTheCats.API.Services;

public class CatService : ICatService
{
    private readonly ICatRepository _catRepository;
    private readonly ICatApiService _catApiService;

    public CatService(ICatRepository catRepository, ICatApiService catApiService)
    {
        _catRepository = catRepository;
        _catApiService = catApiService;
    }

    public async Task<CatPaginatedResponse> GetAllCatsPaginatedAsync(int page, int pageSize)
    {
        var cats = await _catRepository.GetCatsAsync();

        var response = PaginateCats(cats, page, pageSize);
        return response;
    }

    public async Task<CatPaginatedResponse> GetAllCatsPaginatedAsync(string tag, int page, int pageSize)
    {
        var cats = await _catRepository.GetCatsByTagAsync(tag);

        var response = PaginateCats(cats, page, pageSize);
        return response;
    }    

    public async Task<CatDto> GetCatByIdAsync(int id)
    {
        var cat = await _catRepository.GetCatByIdAsync(id);

        return MapCatToDto(cat);        
    }

    public async Task<CatDto> GetCatByCatIdAsync(string catId)
    {
        var cat = await _catRepository.GetCatByIdAsync(catId);

        return MapCatToDto(cat);        
    }    

    public async Task<byte[]?> GetCatImageAsync(string catId)
    {
        var cat = await _catRepository.GetCatByIdAsync(catId);
        return cat?.ImageData;
    }

    public async Task AddCatsAsync(IEnumerable<Cat> cats) => await _catRepository.AddCatsAsync(cats);

    public async Task CacheCatImagesAsync()
    {
        var cats = await _catRepository.GetCatsAsync();
        var catsWithoutImages = cats.Where(c => c.ImageData == null).ToList();

        foreach (var cat in catsWithoutImages)
        {
            try
            {
                var imageBytes = await _catApiService.DownloadImageAsync(cat.ImageUrl);
                cat.ImageData = imageBytes;
                await _catRepository.UpdateCatAsync(cat);
            }
            catch
            {
                Console.WriteLine($"Failed to download image for CatId: {cat.Id}");
            }
        }
    }

    public async Task UpdateCatsAsync(IEnumerable<Cat> cats) => await _catRepository.UpdateCatsAsync(cats);

    public async Task FetchAndStoreUniqueCatsAsync()
    {
        // Fetch cats from the API
        var fetchedCats = await _catApiService.FetchCatImagesAsync(true);

        if (fetchedCats == null || fetchedCats.Count == 0)
            return;
        
        var existingCatIds = await _catRepository.GetExistingCatIdsAsync();

        // Filter out duplicates before inserting
        var newCats = fetchedCats
            .Where(cat => !existingCatIds.Contains(cat.CatId))
            .ToList();

        if (newCats.Count > 0)
        {
            await _catRepository.AddCatsAsync(newCats);
        }        
    }

    private CatPaginatedResponse PaginateCats(List<Cat> allCats, int page, int pageSize)
    {
        int totalCats = allCats.Count;
        int totalPages = (int)Math.Ceiling((double)totalCats / pageSize);

        // Ensure page is within range
        if (page > totalPages)
        {
            page = totalPages > 0 ? totalPages : 1;
        }

        List<CatDto> paginatedCats = allCats
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => MapCatToDto(c))
            .ToList();

        return new CatPaginatedResponse
        {
            Cats = paginatedCats,
            TotalPages = totalPages,
            CurrentPage = page
        };
    }

    private static CatDto? MapCatToDto(Cat cat) 
    {
        if(cat is null) return null;

        return new CatDto
        {
            CatId = cat.CatId,
            Height = cat.Height,
            Width = cat.Width,
            ImageUrl = cat.ImageUrl,
            Created = cat.Created,
            Tags = cat.Tags.Select(t => t.Name).ToList()
        };
    }
}
