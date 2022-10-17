using Contacts.Domain.Contact;
using Eventuous;
using static Contacts.Application.ContactCommands;

namespace Contacts.Application;

public class ContactManagerService : ApplicationService<Contact, ContactState, ContactId>
{
    public ContactManagerService(IAggregateStore store, AggregateFactoryRegistry? factoryRegistry = null, StreamNameMap? streamNameMap = null) : base(store, factoryRegistry, streamNameMap)
    {
        OnNew<RegisterNewContact>(command => command.ContactId,
            (contact, command) => contact.RegisterNewContact(command.Name));

        OnExisting<AddEmailAddress>(command => command.ContactId,
            (contact, command) => contact.AddEmailAddress(new EmailAddress(command.EmailAddress)));

        OnExisting<RemoveEmailAddress>(command => command.ContactId,
            (contact, command) => contact.RemoveEmailAddress(new EmailAddress(command.EmailAddress)));
        
        OnExisting<AddPhoneNumber>(command => command.ContactId,
            (contact, command) => contact.AddPhoneNumber(new PhoneNumber(command.PhoneNumberId, command.CountryCode, command.Number)));

        OnExisting<RemovePhoneNumber>(command => command.ContactId,
            (contact, command) => contact.RemovePhoneNumber(command.PhoneNumberId));

        OnExisting<ChangeDescription>(command => command.ContactId,
            (contact, command) => contact.ChangeDescription(command.Description));
    }
}