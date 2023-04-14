using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using Management.Domain.Entities;
using Management.Domain.ValueObjects;
using MediatR;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace Management.Application.Features.Commands.Company.CompleteData;

public class CompleteDataHandler : ICommandHandler<CompleteDataCommand, Unit>
{
    private readonly ICurrentUser _currentUser;
    private readonly ICompanyRepository _companyRepository;

    public CompleteDataHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _companyRepository = unitOfWork?.CompanyRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Unit> Handle(CompleteDataCommand request, CancellationToken cancellationToken)
    {
        var result = await _companyRepository.CompanyNameExistsAsync(request.CompanyName);
        if (result)
        {
            throw new BadRequestException(Messages.NameExistsMessageException, Titles.NameExistsTitle);
        }

        var company = (await _companyRepository.GetCompanyByOwnerCodeAsync(_currentUser.VerificationCode!))?.Map();
        if (company == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Company"),
                Titles.MethodFailedTitle("GetCompanyByOwnerCodeAsync"));
        }

        var name = CompanyName.Create(request.CompanyName);
        var openingHours = request.From.HasValue && request.To.HasValue
            ? OpeningHours.Create(request.From!.Value, request.To!.Value)
            : null;
        var address = Address.Create(request.City, request.Street, request.NumberStreet, request.PostalCode);
        var contact = Contact.Create(request.PhoneNumber, request.Email);
        var communicationData = CommunicationData.Create(address, contact);

        company.UpdateData(name, openingHours, communicationData);

        await _companyRepository.CompleteDataAsync(company);

        return Unit.Value;
    }
}