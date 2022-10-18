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
    public async Task<IEnumerable<ContactDocument>> GetContacts( CancellationToken cancellationToken, int page = 0, int pageSize = 10)
    {
        var contacts = await _database.GetCollection<ContactDocument>("Contact").AsQueryable()
            .Skip(pageSize * page)
            .Take(pageSize)
            .ToAsyncEnumerable()
            .ToListAsync(cancellationToken);
        return contacts;
    }

    [HttpGet]
    [Route("{contactId}")]
    public async Task<ContactDocument> GetContact([FromRoute] Guid contactId, CancellationToken cancellationToken)
    {
        return await _database.GetCollection<ContactDocument>("Contact")
            .FindAsync(document => document.Id == contactId.ToString(), default, cancellationToken)
            .Result.FirstOrDefaultAsync(cancellationToken);
    }
}