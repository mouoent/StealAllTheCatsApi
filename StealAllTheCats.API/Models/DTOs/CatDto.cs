namespace StealAllTheCats.API.Models.DTOs;

public class CatDto
{
    public string CatId { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public string ImageUrl { get; set; } = string.Empty;        
    public DateTime Created { get; set; }
    public List<string> Tags { get; set; } = new();
}
