using System.Collections.Immutable;
using Api;
using Api.Emails;
using Api.Model;
using AutoFixture;
using Hangfire;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<RegistrationsRepository>();
builder.Services.AddSingleton<RegistrationCreator>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<RegistrationType>()
    .AddDataLoader<RegistrationEmailsDataLoader>()
    .AddMutationType<Mutation>()
    .AddInMemorySubscriptions()
    .AddSubscriptionType<Subscription>()
    .AddCacheControl()
    .UseAutomaticPersistedQueryPipeline()
    .AddInMemoryQueryStorage();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddHangfire(c => c.UseMemoryStorage());
builder.Services.AddHangfireServer();
var app = builder.Build();
var backgroundJobClient = app.Services.GetService<IBackgroundJobClient>();
//backgroundJobClient.Enqueue<RegistrationCreator>(x => x.Create());

app.UseCors();
app.UseWebSockets();
app.MapGraphQL();
app.Run();


public class RegistrationType : ObjectType<Registration>
{
    protected override void Configure(IObjectTypeDescriptor<Registration> descriptor)
    {
        descriptor
            .Field("emails")
            .Resolve<Email[]>(async (cx, ct) =>
            {
                await Task.Delay(1000, ct);
                var result = new Fixture().CreateMany<Email>(5).ToArray();
                return result;
            });

       /* descriptor
            .Field("emailsWithLoader")
            .Resolve<Email[]?>(async (cx, ct) =>
            {
                var registration = cx.Parent<Registration>();
                var loader = cx.BatchDataLoader()
                var res = await loader.LoadAsync(registration.Id, ct);
                return (Email[])res;
            });*/

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

public class RegistrationEmailsDataLoader : BatchDataLoader<int, Email[]>
{
    public RegistrationEmailsDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions? options = null) : base(
        batchScheduler, options)
    {
    }

    protected async override Task<IReadOnlyDictionary<int, Email[]>> LoadBatchAsync(IReadOnlyList<int> keys,
        CancellationToken cancellationToken)
    {
        var result = new Dictionary<int, Email[]>();
        await Task.Delay(1000);
        foreach (var t in keys)
        {
            var res = new Fixture().CreateMany<Email>(10).ToArray();
            result.Add(t,res);
        }
        return result.ToImmutableDictionary();
    }
}