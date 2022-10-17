using System.Collections.Immutable;
using Eventuous.Projections.MongoDB.Tools;

namespace Contacts.Application.Queries;

public record ContactDocument : ProjectedDocument
{
    public ContactDocument(string Id) : base(Id)
    {
    }
    
    public string                             Name         { get; init; }
    public ImmutableList<string>              Emails       { get; init; }
    public ImmutableList<PhoneNumberDocument> PhoneNumbers { get; init; }
    public string                             Description  { get; init; }
}

public record PhoneNumberDocument
{
    public string Id { get; init; }
    public string CountryCode { get; init; }
    public string Number { get; init; }
}