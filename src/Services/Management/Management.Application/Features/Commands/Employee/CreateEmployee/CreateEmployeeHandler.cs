using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using Management.Application.Generators;
using Management.Domain.ValueObjects;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace Management.Application.Features.Commands.Employee.CreateEmployee;

public class CreateEmployeeHandler : ICommandHandler<CreateEmployeeCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IDepartmentRepository _departmentRepository;

    public CreateEmployeeHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _departmentRepository = unitOfWork.DepartmentRepository;
    }

    public async Task<string> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var department = (await _departmentRepository.GetDepartmentByIdAsync(request.DepartmentId))?.Map();

        if (department == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Department"),
                Titles.MethodFailedTitle("GetDepartmentByIdAsync"));
        }

        var code = CodeGenerators.PersonCompanyCodeGenerate(_currentUser.OrganizationCode!);
        var name = request.Name;
        var surname = request.Name;
        var birthday = request.Birthday;
        var personIdentifier = request.PersonIdentifier;
        var address = Address.Create(request.City, request.Street, request.NumberStreet, request.PostalCode);
        var contact = Contact.Create(request.PhoneNumber, request.Email);
        var leader = request.LeaderContact ?? _currentUser.Email;

        department.AddNewEmployee(leader, code, name, surname, birthday, personIdentifier, address, contact);

        await _departmentRepository.AddNewEmployeeAsync(department);
        await _unitOfWork.CompleteAsync(department);

        return code;
    }
}