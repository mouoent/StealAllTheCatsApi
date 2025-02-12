using Moq;
using StealAllTheCats.API.Models.Entities;

namespace StealAllTheCats.Tests.API.CatServiceTests.UnitTests;

public class CatServiceUnitTests : CatServiceUnitTestBase
{
    [Fact]
    public async Task FetchAndStoreUniqueCatsAsync_ShouldStoreNewCats()
    {
        // Arrange
        var existingCats = new HashSet<string> { "cat1", "cat2" };
        var newCats = new List<Cat>
        {
            new() { CatId = "cat3", ImageUrl = "http://image3.com" },
            new() { CatId = "cat4", ImageUrl = "http://image4.com" }
        };

        _catRepositoryMock.Setup(repo => repo.GetExistingCatIdsAsync())
            .ReturnsAsync(existingCats);
        _catApiServiceMock.Setup(api => api.FetchCatImagesAsync(true))
            .ReturnsAsync(newCats);
        _catRepositoryMock.Setup(repo => repo.AddCatsAsync(It.IsAny<IEnumerable<Cat>>()))
            .Returns(Task.CompletedTask);

        // Act
        await _catService.FetchAndStoreUniqueCatsAsync();

        // Assert
        _catRepositoryMock.Verify(repo => repo.AddCatsAsync(It.Is<IEnumerable<Cat>>(c => c.Count() == 2)), Times.Once);
    }

    [Fact]
    public async Task FetchAndStoreUniqueCatsAsync_ShouldNotStoreDuplicates()
    {
        // Arrange
        var existingCats = new HashSet<string> { "cat1", "cat2", "cat3" };
        var newCats = new List<Cat>
        {
            new() { CatId = "cat1", ImageUrl = "http://image1.com" }, // Duplicate
            new() { CatId = "cat4", ImageUrl = "http://image4.com" }  // New
        };

        _catRepositoryMock.Setup(repo => repo.GetExistingCatIdsAsync())
            .ReturnsAsync(existingCats);
        _catApiServiceMock.Setup(api => api.FetchCatImagesAsync(true))
            .ReturnsAsync(newCats);

        // Act
        await _catService.FetchAndStoreUniqueCatsAsync();

        // Assert
        _catRepositoryMock.Verify(repo => repo.AddCatsAsync(It.Is<IEnumerable<Cat>>(c => c.Count() == 1)), Times.Once);
    }
}