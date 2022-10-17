using Eventuous;
using Eventuous.Projections.MongoDB;
using MongoDB.Driver;
using static Contacts.Domain.Contact.ContactEvents;

namespace Contacts.Api.Application.Queries;

public class ContactStateProjection : MongoProjection<ContactDocument>
{
    public ContactStateProjection(IMongoDatabase database, TypeMapper? typeMap = null) : base(database, typeMap)
    {
        On<V1.NewContactRegistered>(stream => stream.GetId(),
            (ctx, update) => update
                .SetOnInsert(document => document.Id, ctx.Stream.GetId())
                .Set(document => document.Name, ctx.Message.Name));

        On<V1.EmailAddressAdded>(stream => stream.GetId(),
            (ctx, update) => update
                .AddToSet(document => document.Emails, ctx.Message.EmailAddress.Email));

        On<V1.EmailAddressRemoved>(stream => stream.GetId(), (ctx, update) => update
            .Pull(document => document.Emails, ctx.Message.EmailAddress.Email));

        On<V1.PhoneNumberAdded>(stream => stream.GetId(),
            (ctx, update) => update
                .AddToSet(document => document.PhoneNumbers,
                    new PhoneNumberDocument
                    {
                        Id = ctx.Message.PhoneNumber.Id.ToString(),
                        CountryCode = ctx.Message.PhoneNumber.CountryCode,
                        Number = ctx.Message.PhoneNumber.Value
                    }));

        On<V1.PhoneNumberRemoved>(stream => stream.GetId(),
            (ctx, update) => update.PullFilter(document => document.PhoneNumbers,
                document => document.Id == ctx.Message.PhoneNumber.Id.ToString()));

        On<V1.DescriptionChanged>(stream => stream.GetId(),
            (ctx, update) => update.Set(document => document.Description, ctx.Message.Description));
    }
}