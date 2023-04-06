using PIIDataClient;

namespace Api.Model;

public class TicketType : ObjectType<Ticket>
{
    protected override void Configure(IObjectTypeDescriptor<Ticket> descriptor)
    {
        descriptor
            .Field("client")
            .Resolve<Client?>(async (cx, ct) =>
            {
                var registration = cx.Parent<Registration>();
                var client = cx.Service<ClientService>().GetClientById(registration.ClientId);
                return client;
            });
        base.Configure(descriptor);
    }

}