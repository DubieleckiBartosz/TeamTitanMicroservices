using Calculator.Application.Constants;
using Calculator.Application.Contracts;
using Calculator.Application.Features.Account.ViewModels;
using Calculator.Application.ReadModels.AccountReaders;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Mappings;
using Shared.Implementations.Services;

namespace Calculator.Application.Features.Account.Queries.GetAccountsBySearch;

public class GetAccountsBySearchQueryHandler : IQueryHandler<GetAccountsBySearchQuery, AccountSearchViewModel>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUser _currentUser;

    public GetAccountsBySearchQueryHandler(IAccountRepository accountRepository, ICurrentUser currentUser)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public async Task<AccountSearchViewModel> Handle(GetAccountsBySearchQuery request, CancellationToken cancellationToken)
    {
        var accountId = request.AccountId;
        var countingType = request.CountingType;
        var accountStatus = request.AccountStatus;
        var expirationDateFrom = request.ExpirationDateFrom;
        var expirationDateTo = request.ExpirationDateTo;
        var activatedBy = request.ActivatedBy;
        var deactivatedBy = request.DeactivatedBy;
        var hourlyRateFrom = request.HourlyRateFrom;
        var hourlyRateTo = request.HourlyRateTo;
        var settlementDayMonth = request.SettlementDayMonth;
        var balanceFrom = request.BalanceFrom;
        var balanceTo = request.BalanceTo;
        var sortType = request.Sort.Type;
        var sortName = request.Sort.Name;
        var pageNumber = request.PageNumber;
        var pageSize = request.PageSize;
        var companyCode = _currentUser.OrganizationCode!;

        var response = await _accountRepository.GetAccountsBySearchAsync(accountId, countingType, accountStatus,
            expirationDateFrom, expirationDateTo, activatedBy, deactivatedBy, hourlyRateFrom, hourlyRateTo, settlementDayMonth, balanceFrom,
            balanceTo, sortType, sortName, pageNumber, pageSize, companyCode);

        if (response == null)
        {
            throw new NotFoundException(Messages.AccountsNotFoundMessage, Titles.DataNotFoundTitle);
        }

        var data = response.Data;
        var accounts = Mapping.MapList<AccountReader, AccountViewModel>(data);

        return new AccountSearchViewModel(response.TotalCount, accounts);
    }
}