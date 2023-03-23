namespace Calculator.Application.Features.Account.ViewModels.AccountViews;

public record AccountSearchViewModel(int TotalCount, List<AccountViewModel> Accounts);