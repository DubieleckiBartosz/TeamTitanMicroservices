using Shared.Domain.Abstractions;

namespace Management.Application.NotificationHandlers.ProcessNotifications;

public record NewPaymentMonthDayProcess(Guid AccountId, int PaymentMonthDay) : IEvent;