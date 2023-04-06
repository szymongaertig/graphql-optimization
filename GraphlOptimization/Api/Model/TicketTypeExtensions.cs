using HotChocolate.Caching;
using PIIDataClient;

namespace Api.Model;

public class TicketType : ObjectType<Ticket>
{
    protected override void Configure(IObjectTypeDescriptor<Ticket> descriptor)
    {
        descriptor
            .Field("client")
            .CacheControl(100_000, CacheControlScope.Private)
            .Resolve<Client?>(async (cx, ct) =>
            {
                var ticket = cx.Parent<Ticket>();
                var client = cx.Service<ClientService>().GetClientById(ticket.ClientId);
                return client;
            });
        base.Configure(descriptor);
    }

}