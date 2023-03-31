using MediatR;

namespace Management.Application.Features.Company.Commands.InitCompany;

public record InitCompanyCommand() : ICommand<Unit>;