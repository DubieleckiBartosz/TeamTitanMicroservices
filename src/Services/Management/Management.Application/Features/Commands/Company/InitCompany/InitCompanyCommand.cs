using MediatR;

namespace Management.Application.Features.Commands.Company.InitCompany;

public record InitCompanyCommand() : ICommand<Unit>;