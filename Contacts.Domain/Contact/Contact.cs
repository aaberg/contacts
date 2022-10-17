using Eventuous;
using static Contacts.Domain.Contact.ContactEvents;

namespace Contacts.Domain.Contact;

public class Contact : Aggregate<ContactState>
{
    public void RegisterNewContact(string name)
    {
        EnsureDoesntExist();
        Apply(new V1.NewContactRegistered(name));
    }

    public void AddEmailAddress(EmailAddress emailAddress)
    {
        EnsureExists();
        
        // Throw if email address already exists
        if (State.EmailAddresses.Exists(address => address.Email == emailAddress.Email))
        {
            throw new DomainException("Email address already exists");
        }
        
        Apply(new V1.EmailAddressAdded(emailAddress));
    }

    public void RemoveEmailAddress(EmailAddress emailAddress)
    {
        EnsureExists();
        
        if (!State.EmailAddresses.Contains(emailAddress))
        {
            throw new DomainException("Cannot delete email address that does not exist");
        }
        
        Apply(new V1.EmailAddressRemoved(emailAddress));
    }
    
    public void AddPhoneNumber(PhoneNumber phoneNumber) 
    {
        EnsureExists();
        
        if (State.PhoneNumbers.Contains(phoneNumber))
        {
            throw new DomainException("Phone number already exists");
        }
        
        Apply(new V1.PhoneNumberAdded(phoneNumber));
    }
    
    public void RemovePhoneNumber(Guid phoneNumberId)
    {
        EnsureExists();

        var phoneNumber = State.PhoneNumbers.SingleOrDefault(number => number.Id == phoneNumberId);
        
        if (phoneNumber == default)
        {
            throw new DomainException("Cannot delete phone number that does not exist");
        }
        
        Apply(new V1.PhoneNumberRemoved(phoneNumber));
    }
    
    public void ChangeDescription(string description)
    {
        EnsureExists();
        Apply(new V1.DescriptionChanged(description));
    }
}