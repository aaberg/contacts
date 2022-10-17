using Contacts.Domain.Contact;
using Eventuous;
using FluentAssertions;

namespace Contacts.Domain.Tests.Contact;

public class ContactTests
{
    
    [Fact]
    public void GivenNonExistingContact_RegisterContactWithValidParams_ThenSucceeds()
    {
        var contactName = "Testing Testingson";

        Domain.Contact.Contact contact = new Domain.Contact.Contact();
        contact.RegisterNewContact(contactName);
        
        contact.State.Name.Should().BeEquivalentTo(contactName);
        contact.State.EmailAddresses.Should().BeEmpty();
        contact.State.PhoneNumbers.Should().BeEmpty();
    }

    [Fact]
    public void GivenNonExistingContact_AddEmailOrPhoneNumber_ThenThrows()
    {
        var contact = new Domain.Contact.Contact();

        var addEmailAction = () => contact.AddEmailAddress(new EmailAddress("test@test.com"));
        var addPhoneNumberAction = () => contact.AddPhoneNumber(new PhoneNumber(Guid.NewGuid(), "+47", "123456789"));

        addEmailAction.Should().Throw<DomainException>();
        addPhoneNumberAction.Should().Throw<DomainException>();
    }

    [Fact]
    public void GivenExistingContact_AddAndRemoveEmailOrPhoneNumber_ThenSucceeds()
    {
        var contact = new Domain.Contact.Contact();
        contact.RegisterNewContact("Testing Testingson");
        var email = new EmailAddress("test@test.com");
        var phoneNumber = new PhoneNumber(Guid.NewGuid(), "+47", "123456789");

        contact.AddEmailAddress(email);
        contact.AddPhoneNumber(phoneNumber);

        contact.State.EmailAddresses.Should().Contain(email);
        contact.State.PhoneNumbers.Should().Contain(phoneNumber);

        contact.RemoveEmailAddress(email with { });
        contact.RemovePhoneNumber(phoneNumber.Id);

        contact.State.EmailAddresses.Should().BeEmpty();
        contact.State.PhoneNumbers.Should().BeEmpty();
    }
}