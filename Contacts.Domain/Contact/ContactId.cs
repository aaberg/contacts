using Eventuous;

namespace Contacts.Domain.Contact;

public record ContactId(string Value) : AggregateId(Value)
{
    public static implicit operator Guid(ContactId contactId) => Guid.Parse(contactId);
    public static implicit operator ContactId(Guid guidId)    => new ContactId(guidId.ToString());
}