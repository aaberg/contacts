using Contacts.Application;
using Contacts.Domain.Contact;
using Eventuous;
using Eventuous.AspNetCore.Web;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.HttpApi;

[Route("api/contact")]
public class CommandApi : CommandHttpApiBase<Contact>
{
    public CommandApi(IApplicationService<Contact> service) : base(service)
    {
    }
    
    [HttpPost]
    [Route("registernew")]
    public Task<ActionResult<Result>> RegisterNewContact([FromBody] ContactCommands.RegisterNewContact command, CancellationToken cancellationToken) => 
        Handle(command, cancellationToken);
    
    [HttpPost]
    [Route("addemailaddress")]
    public Task<ActionResult<Result>> AddEmailAddress([FromBody] ContactCommands.AddEmailAddress command, CancellationToken cancellationToken) => 
        Handle(command, cancellationToken);
    
    [HttpPost]
    [Route("removeemailaddress")]
    public Task<ActionResult<Result>> RemoveEmailAddress([FromBody] ContactCommands.RemoveEmailAddress command, CancellationToken cancellationToken) => 
        Handle(command, cancellationToken);
    
    [HttpPost]
    [Route("addphonenumber")]
    public Task<ActionResult<Result>> AddPhoneNumber([FromBody] ContactCommands.AddPhoneNumber command, CancellationToken cancellationToken) => 
        Handle(command, cancellationToken);
    

    [HttpPost]
    [Route("removephonenumber")]
    public Task<ActionResult<Result>> RemovePhoneNumber([FromBody] ContactCommands.RemovePhoneNumber command, CancellationToken cancellationToken) => 
        Handle(command, cancellationToken);
    
    [HttpPost]
    [Route("changedescription")]
    public Task<ActionResult<Result>> ChangeDescription([FromBody] ContactCommands.ChangeDescription command, CancellationToken cancellationToken) => 
        Handle(command, cancellationToken);
}