﻿using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace Management.Application.Features.Company.Commands.UpdateContact;

public class UpdateCompanyContactHandler : ICommandHandler<UpdateCompanyContactCommand, Unit>
{
    private readonly ICurrentUser _currentUser;
    private readonly ICompanyRepository _companyRepository;

    public UpdateCompanyContactHandler(ICurrentUser currentUser, ICompanyRepository companyRepository)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
    }
    public async Task<Unit> Handle(UpdateCompanyContactCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetCompanyByCodeAsync(_currentUser.OrganizationCode!);
        if (company == null)
        {
            throw new NotFoundException("Company not found by organization code", "Company not found");
        }
        var phoneNumber = request.PhoneNumber;
        var email = request.PhoneNumber;
        var city = request.PhoneNumber;
        var street = request.PhoneNumber;
        var numberStreet = request.PhoneNumber;
        var postalCode = request.PhoneNumber;

        company.UpdateCommunicationData(phoneNumber, email, city, street, numberStreet, postalCode);

        return Unit.Value;
    }
}