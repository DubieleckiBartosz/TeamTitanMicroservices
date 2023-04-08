using Shared.Domain.Abstractions;

namespace Management.Domain.Events;

public record DayOffRequestCreated(string Leader, string EmployeeFullName, string EmployeeCode) : IDomainNotification;