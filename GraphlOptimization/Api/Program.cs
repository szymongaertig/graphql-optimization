using Api;
using Api.DataLoaders;
using Api.Emails;
using Api.Model;
using AutoFixture;
using Hangfire;
using Hangfire.MemoryStorage;
using HotChocolate.Language;
using PIIDataClient;
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
builder.Services.AddSingleton<ClientService>();

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
    .AddType<TicketType>()
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

