using Microsoft.EntityFrameworkCore;
using StealAllTheCats.API.Interfaces;
using StealAllTheCats.API.Models.Entities;
using StealAllTheCats.API.Persistence;

namespace StealAllTheCats.API.Repositories;

public class CatRepository : ICatRepository
{
    private readonly StealAllTheCatsContext _context;

    public CatRepository(StealAllTheCatsContext context)
    {
        _context = context;
    }

    public async Task<List<Cat>> GetCatsAsync() 
        => await _context.Cats.Include(c => c.Tags).ToListAsync();
                        
    public async Task<List<Cat>> GetCatsByTagAsync(string tag) 
        => await _context.Cats
        .Include(c => c.Tags)
        .Where(c => c.Tags.Any(t => t.Name.Contains(tag)))
        .ToListAsync();

    public async Task<HashSet<string>> GetExistingCatIdsAsync() 
        => await _context.Cats.Select(c => c.CatId).ToHashSetAsync();
    
    public async Task<Cat?> GetCatByIdAsync(int id)
        => await _context.Cats.Include(c => c.Tags).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Cat?> GetCatByIdAsync(string catId)
        => await _context.Cats.Include(c => c.Tags).FirstOrDefaultAsync(c => c.CatId == catId);

    public async Task<List<Cat>> GetCatsWithoutImagesAsync()
        => await _context.Cats.Include(c => c.Tags).Where(c => c.ImageData == null).ToListAsync();            

    public async Task AddCatsAsync(IEnumerable<Cat> cats)
    {
        foreach (var cat in cats)
        {
            if (!_context.Cats.Any(c => c.CatId == cat.CatId))
            {
                _context.Cats.Add(cat);
            }
        }
        await _context.SaveChangesAsync();
    }

    /* public async Task<byte[]?> GetCatImageAsync(int id)
     {
         var cat = await _context.Cats.FirstOrDefaultAsync(c => c.Id == id);
         return cat?.ImageData;
     }*/

    /*  public async Task<string?> GetCatImageUrlAsync(int id)
      {
          var cat = await _context.Cats.FirstOrDefaultAsync(c => c.Id == id);
          return cat?.ImageUrl;
      }*/

    public async Task UpdateCatAsync(Cat cat)
    {
        _context.Cats.Update(cat);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCatsAsync(IEnumerable<Cat> cats)
    {
        _context.Cats.UpdateRange(cats);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
