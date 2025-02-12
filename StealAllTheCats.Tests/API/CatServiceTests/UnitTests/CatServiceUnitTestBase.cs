using Moq;
using StealAllTheCats.API.Interfaces;
using StealAllTheCats.API.Services;

namespace StealAllTheCats.Tests.API.CatServiceTests.UnitTests;

public abstract class CatServiceUnitTestBase
{
    protected readonly Mock<ICatRepository> _catRepositoryMock;
    protected readonly Mock<ICatApiService> _catApiServiceMock;    

    protected readonly CatService _catService;

    protected CatServiceUnitTestBase()
    {
        _catRepositoryMock = new Mock<ICatRepository>();
        _catApiServiceMock = new Mock<ICatApiService>();

        _catService = new CatService(_catRepositoryMock.Object, _catApiServiceMock.Object);
    }
}
