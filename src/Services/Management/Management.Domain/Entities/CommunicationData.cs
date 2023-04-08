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

    public CommunicationData(Address address, Contact contact, int version) : this(address, contact)
    { 
        Version = version;
    }

    public static CommunicationData Create(Address address, Contact contact)
    {
        return new CommunicationData(address, contact);
    } 
    public static CommunicationData Load(Address address, Contact contact, int version)
    {
        return new CommunicationData(address, contact, version);
    }

    public void UpdateContact(Contact contact)
    {
        this.Contact = contact;
        IncrementVersion();
    }

    public void UpdateAddress(Address address)
    {
        this.Address = address;
        IncrementVersion();
    }
}