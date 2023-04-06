using Api.DataLoaders;
using Api.Emails;
using AutoFixture;
using PIIDataClient;

namespace Api.Model;

public class RegistrationType : ObjectType<Registration>
{
    protected override void Configure(IObjectTypeDescriptor<Registration> descriptor)
    {
        descriptor
            .Field("client")
            .Resolve<Client?>(async (cx, ct) =>
            {
                var registration = cx.Parent<Registration>();
                var client = cx.Service<ClientService>().GetClientById(registration.ClientId);
                return client;
            });
    
        descriptor
            .Field("emails")
            .Resolve<Email[]?>(async (cx, ct) =>
            {
                var registration = cx.Parent<Registration>();
                var emailService = cx.Service<EmailsService>();
                return await emailService.GetRegistrationEmails(registration.Id);
            });

        descriptor
            .Field("emailsWithLoader")
            .Resolve<Email[]?>(async (cx, ct) =>
            {
                var registration = cx.Parent<Registration>();
                var loader = cx.DataLoader<RegistrationEmailsDataLoader>();
                var result = await loader.LoadAsync(registration.Id, ct);
                return result;
            });

        descriptor
            .Field("emailsStream")
            .Resolve<IAsyncEnumerable<Email>>(async (cx, t) =>
            {
                var registration = cx.Parent<Registration>();
                return GetRegistrationEmailsStream(registration.Id);
            });

        base.Configure(descriptor);
    }

    private static async IAsyncEnumerable<Email> GetRegistrationEmailsStream(Guid registrationId)
    {
        var result = new Fixture().CreateMany<Email>(10);
        foreach (var email in result)
        {
            yield return email;
            await Task.Delay(500);
        }
    }
}