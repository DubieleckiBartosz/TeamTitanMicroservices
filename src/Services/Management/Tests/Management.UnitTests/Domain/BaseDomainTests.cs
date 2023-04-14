using AutoFixture;

namespace Management.UnitTests.Domain;

public class BaseDomainTests
{
    protected readonly Fixture Fixture;
    public BaseDomainTests()
    {
        Fixture = new Fixture();
    }
}