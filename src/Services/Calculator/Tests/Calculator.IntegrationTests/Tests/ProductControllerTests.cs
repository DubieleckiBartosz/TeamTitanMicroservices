using Calculator.Application.Contracts.Repositories;
using Calculator.Application.Features.Product.ViewModels;
using Calculator.Application.ReadModels.ProductReaders;
using Calculator.Domain.Product;
using Calculator.IntegrationTests.Generators.Product;
using Calculator.IntegrationTests.Setup;
using Calculator.IntegrationTests.Setup.FakeRepositories;
using Moq;
using Shared.Implementations.EventStore;
using Shared.Implementations.Search;

namespace Calculator.IntegrationTests.Tests;

public class ProductControllerTests : BaseSetup
{
    private readonly ProductReaderFakeRepository _productReaderRepository;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IStore> _storeMock;

    public ProductControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
        _productRepositoryMock = Mocks.ProductRepositoryMock;
        _productReaderRepository = FakeRepositories.GetProductReaderFakeRepository();
        _storeMock = Mocks.Store;
    }

    [Fact]
    public async Task Should_Find_Product_With_Details_By_Id()
    {
        //Arrange
        var eventReader = _productReaderRepository.GetFirstProductWithHistory();
        var product = eventReader!.Reader;

        _productRepositoryMock.Setup(_ => _.GetProductWithHistoryAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(product);

        //Act
        var response = await ClientCall<object>(null, HttpMethod.Get, Urls.GetProductByIdPath + $"/{product.Id}");
        var responseData = await ReadFromResponse<ProductDetailsViewModel>(response);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(responseData);
        Assert.True(responseData!.PriceHistory.Any());
    }

    [Fact]
    public async Task Should_Returns_Products_By_Search()
    {
        //Arrange
        var products = _productReaderRepository.GetAllProducts().ToList();
        var listCount = products.Count;
        var searchResponse = ResponseSearchList<ProductReader>.Create(products, listCount);
        var request = Fixture.GetSearchProductsParameters();

        _productRepositoryMock.Setup(_ => _.GetProductsBySearchAsync(It.IsAny<string?>(), It.IsAny<decimal?>(),
                It.IsAny<decimal?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(), It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(searchResponse);

        //Act
        var response = await ClientCall(request, HttpMethod.Post, Urls.SearchProductsPath);
        var responseData = await ReadFromResponse<ProductSearchViewModel>(response);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(listCount, responseData!.TotalCount);
    }

    [Fact]
    public async Task Should_Create_New_Product()
    {
        //Arrange  
        var request = Fixture.GetCreateNewProductParameters();

        _productRepositoryMock.Setup(_ => _.ProductSkuExistsAsync(It.IsAny<string>())).ReturnsAsync(() => false);

        //Act
        var response = await ClientCall(request, HttpMethod.Post, Urls.CreateNewProductPath);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        _productRepositoryMock.Verify(_ => _.AddAsync(It.IsAny<ProductReader>()), Times.Once);
    }

    [Fact]
    public async Task Should_Update_Product_Availability()
    {
        //Arrange 
        var eventReader = _productReaderRepository.GetFirstNotAvailableProduct();
        var product = eventReader!.Reader;
        var streamList = eventReader.ToStreamList<PieceworkProduct>(product.Id);
        var request = Fixture.GetUpdateAvailabilityParameters(product.Id);

        SetupStore(streamList);
        _productRepositoryMock.Setup(_ => _.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);

        //Act
        var response = await ClientCall(request, HttpMethod.Put, Urls.UpdateAvailabilityPath);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        _productRepositoryMock.Verify(v => v.UpdateAvailabilityAsync(It.IsAny<ProductReader>()), Times.Once);
    }

    [Fact]
    public async Task Should_Update_Price_Product()
    {
        //Arrange
        var eventReader = _productReaderRepository.GetFirstAvailableProduct();
        var product = eventReader!.Reader;
        var streamList = eventReader.ToStreamList<PieceworkProduct>(product.Id);

        var request = Fixture.GetUpdatePriceParameters(product.Id);

        SetupStore(streamList);
        _productRepositoryMock.Setup(_ => _.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);

        //Act
        var response = await ClientCall(request, HttpMethod.Put, Urls.UpdatePriceProductPath);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        _productRepositoryMock.Verify(v => v.UpdatePriceAsync(It.IsAny<ProductReader>()), Times.Once);
    }
}