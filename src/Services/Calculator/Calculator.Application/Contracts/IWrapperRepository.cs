namespace Calculator.Application.Contracts;

public interface IWrapperRepository
{
    IAccountRepository AccountRepository { get; }
    IBonusRepository BonusRepository { get; }
    IProductRepository ProductRepository { get; }
}