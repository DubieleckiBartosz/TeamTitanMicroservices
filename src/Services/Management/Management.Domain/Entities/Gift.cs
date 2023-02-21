using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class Gift : Entity
{
    public Guid GiftExternalId { get; set; }
    public GiftRecipient Recipient { get; set; }
    public bool IsActive { get; set; }

    public Gift(Guid giftExternalId, GiftRecipient recipient)
    {
        GiftExternalId = giftExternalId;
        Recipient = recipient;
        IsActive = true;
    }

    public void IsUsed()
    {
        IsActive = false;
    } 

    //[TODO]
}