using StealAllTheCats.API.Models.DTOs;

namespace StealAllTheCats.API.Models.Responses;

public class CatPaginatedResponse
{
    public List<CatDto> Cats { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}
