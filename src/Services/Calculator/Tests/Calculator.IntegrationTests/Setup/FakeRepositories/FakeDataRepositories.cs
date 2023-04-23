using AutoFixture;

namespace Calculator.IntegrationTests.Setup.FakeRepositories;

public class FakeDataRepositories
{
    private readonly AccountReaderFakeRepository _accountFakeRepository;
    private readonly ProductReaderFakeRepository _productFakeRepository;

    public FakeDataRepositories()
    {
        var fixture = new Fixture();
        _accountFakeRepository = new AccountReaderFakeRepository(fixture);
        _productFakeRepository = new ProductReaderFakeRepository(fixture);
    }

    public ProductReaderFakeRepository GetProductReaderFakeRepository() => _productFakeRepository;
    public AccountReaderFakeRepository GetAccountReaderFakeRepository() => _accountFakeRepository;

}