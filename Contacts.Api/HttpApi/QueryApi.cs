using Contacts.Api.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Contacts.Api.HttpApi;

[Route("api/contacts")]
public class QueryApi : ControllerBase
{
    private readonly IMongoDatabase _database;

    public QueryApi(IMongoDatabase database)
    {
        _database = database;
    }

    [HttpGet]
    [Route("")]
    public IAsyncEnumerable<ContactDocument> GetContacts( CancellationToken cancellationToken, int page = 0, int pageSize = 10)
    {
        var contacts = _database.GetCollection<ContactDocument>("Contact").AsQueryable()
            .Skip(pageSize * page)
            .Take(pageSize)
            .ToAsyncEnumerable();

        return contacts;
    }
}