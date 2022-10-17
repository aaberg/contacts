namespace Contacts.Api.Application;

public static class ContactCommands
{
    public record RegisterNewContact(Guid ContactId, string Name);
    
    public record AddEmailAddress(Guid ContactId, string EmailAddress);
    
    public record RemoveEmailAddress(Guid ContactId, string EmailAddress);

    public record AddPhoneNumber()
    {
        public Guid   ContactId     { get; init; } = Guid.Empty;
        public Guid   PhoneNumberId { get; init; } = Guid.Empty;
        public string CountryCode   { get; init; } = null!;
        public string Number        { get; init; } = null!;
    }

    public record RemovePhoneNumber(Guid ContactId, Guid PhoneNumberId);
    
    public record ChangeDescription(Guid ContactId, string Description);
}