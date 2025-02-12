namespace StealAllTheCats.API.Models.Entities;

public class Cat
{
    public int Id { get; set; }
    public string CatId { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public byte[]? ImageData { get; set; } // blob
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public List<Tag> Tags { get; set; } = new();
}
