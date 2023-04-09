namespace Management.Application.Models.DataAccessObjects;

public class ContractWithAccountDao : ContractDao
{
    public Guid AccountId { get; init; }
}