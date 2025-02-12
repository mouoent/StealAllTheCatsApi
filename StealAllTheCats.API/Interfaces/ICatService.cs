using StealAllTheCats.API.Models.DTOs;
using StealAllTheCats.API.Models.Entities;
using StealAllTheCats.API.Models.Responses;
using StealAllTheCats.API.Repositories;

namespace StealAllTheCats.API.Interfaces;

public interface ICatService
{
    Task<CatPaginatedResponse> GetAllCatsPaginatedAsync(int page, int pageSize);
    Task<CatPaginatedResponse> GetAllCatsPaginatedAsync(string tag, int page, int pageSize);
    Task<CatDto> GetCatByIdAsync(int id);
    Task<CatDto> GetCatByCatIdAsync(string catId);
    Task<byte[]?> GetCatImageAsync(string catId);
    Task CacheCatImagesAsync();
    Task FetchAndStoreUniqueCatsAsync();
    Task AddCatsAsync(IEnumerable<Cat> cats);
    Task UpdateCatsAsync(IEnumerable<Cat> cats);
}