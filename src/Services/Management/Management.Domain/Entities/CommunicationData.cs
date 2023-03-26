using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class CommunicationData : Entity
{
    public Address Address { get; private set; }
    public Contact Contact { get; private set; }

    private CommunicationData(Address address, Contact contact)
    {
        Address = address;
        Contact = contact;
    }

    public static CommunicationData Create(Address address, Contact contact)
    {
        return new CommunicationData(address, contact);
    }

    public void UpdateContact(Contact contact)
    {
        this.Contact = contact;
    }

    public void UpdateAddress(Address address)
    {
        this.Address = address;
    }
}