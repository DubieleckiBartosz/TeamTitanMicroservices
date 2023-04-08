using Management.Application.Models.DataTransferObjects;

namespace Management.Application.Contracts.Services;

public interface IMessageService
{
    Task SendInitCompanyMessage(string recipient, InitCompanyMessageDto message);
    Task SendNewDayOffRequestMessage(string recipient, NewDayOffRequestMessageDto message);
}