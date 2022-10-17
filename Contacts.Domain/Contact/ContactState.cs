using System.Collections.Immutable;
using Eventuous;
using static Contacts.Domain.Contact.ContactEvents;

namespace Contacts.Domain.Contact;

public record ContactState : AggregateState<ContactState> {
    public String                      Name           { get; init; }
    public ImmutableList<PhoneNumber>  PhoneNumbers   { get; init; }
    public ImmutableList<EmailAddress> EmailAddresses { get; init; }
    public string?                     Description    { get; init; }
    public bool                        Deleted        { get; init; }

    public ContactState()
    {
        On<V1.NewContactRegistered>((state, e) => state with
        {
            Name = e.Name,
            PhoneNumbers = ImmutableList<PhoneNumber>.Empty,
            EmailAddresses = ImmutableList<EmailAddress>.Empty,
            Deleted = false,
        });
        
        On<V1.PhoneNumberAdded>((state, e) => state with
        {
            PhoneNumbers = state.PhoneNumbers.Add(e.PhoneNumber)
        });

        On<V1.PhoneNumberRemoved>((state, e) => state with
        {
            PhoneNumbers = state.PhoneNumbers.Remove(e.PhoneNumber)
        });
        
        On<V1.EmailAddressAdded>((state, e) => state with
        {
            EmailAddresses = state.EmailAddresses.Add(e.EmailAddress)
        });
        
        On<V1.EmailAddressRemoved>((state, e) => state with
        {
            EmailAddresses = state.EmailAddresses.Remove(e.EmailAddress)
        });

        On<V1.ContactDeleted>((state, e) => state with { Deleted = true });
        
        On<V1.DescriptionChanged>((state, e) => state with { Description = e.Description });
    }
}