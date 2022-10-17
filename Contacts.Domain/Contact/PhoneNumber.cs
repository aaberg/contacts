namespace Contacts.Domain.Contact;

public record PhoneNumber(Guid Id, string CountryCode, string Value);