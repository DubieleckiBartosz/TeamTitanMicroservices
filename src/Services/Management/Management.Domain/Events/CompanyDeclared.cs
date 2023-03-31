using Shared.Domain.Abstractions;

namespace Management.Domain.Events;

public record CompanyDeclared(string CompanyCode, string OwnerCode) : IDomainNotification;