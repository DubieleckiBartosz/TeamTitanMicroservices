using AutoFixture;
using Calculator.Application.Parameters.AccountParameters;

namespace Calculator.IntegrationTests.Generators.Account;

public static class AccountQueriesGenerator
{
    public static GetAccountsBySearchParameters GetAccountsSearchParameters(this Fixture fixture)
    {
        return fixture.Create<GetAccountsBySearchParameters>();
    } 
}