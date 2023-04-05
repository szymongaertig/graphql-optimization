using Api;
using Api.DataLoaders;
using Api.Emails;
using Api.Model;
using AutoFixture;
using Hangfire;
using Hangfire.MemoryStorage;
using HotChocolate.Language;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting web application");
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddSingleton<RegistrationsRepository>();
builder.Services.AddSingleton<RegistrationCreator>();
builder.Services.AddSingleton<EmailsService>();

builder.Services
    .AddMemoryCache()
    .AddSha256DocumentHashProvider(HashFormat.Hex)
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<RegistrationType>()
    .AddDataLoader<RegistrationEmailsDataLoader>()
    .AddMutationType<Mutation>()
    .AddInMemorySubscriptions()
    .AddSubscriptionType<Subscription>()
    //.UseAutomaticPersistedQueryPipeline()
    //.AddInMemoryQueryStorage()
    .AddCacheControl()
    .UseQueryCachePipeline();

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
backgroundJobClient.Enqueue<RegistrationCreator>(x => x.Create());

app.UseWebSockets();
app.UseCors();
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