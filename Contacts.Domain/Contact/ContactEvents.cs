using Eventuous;

namespace Contacts.Domain.Contact;

public static class ContactEvents
{
    public static class V1
    {
        [EventType("V1.NewContactRegistered")]
        public record NewContactRegistered(string Name);
        
        [EventType("V1.PhoneNumberAdded")]
        public record PhoneNumberAdded(PhoneNumber PhoneNumber);
        
        [EventType("V1.PhoneNumberRemoved")]
        public record PhoneNumberRemoved(PhoneNumber PhoneNumber);
        
        [EventType("V1.EmailAddressAdded")]
        public record EmailAddressAdded(EmailAddress EmailAddress);
        
        [EventType("V1.EmailAddressRemoved")]
        public record EmailAddressRemoved(EmailAddress EmailAddress);
        
        [EventType("V1.ContactDeleted")]
        public record ContactDeleted();
        
        [EventType("V1.DescriptionChanged")]
        public record DescriptionChanged(string Description);
    }
    
}