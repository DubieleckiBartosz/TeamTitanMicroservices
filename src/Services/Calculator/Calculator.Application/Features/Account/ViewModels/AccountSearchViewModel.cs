namespace Calculator.Application.Features.Account.ViewModels;

public record AccountSearchViewModel(int TotalCount, List<AccountViewModel> Accounts);