using Api.Emails;

namespace Api.DataLoaders;

public class RegistrationEmailsDataLoader : DataLoaderBase<Guid, Email[]>
{
    private readonly EmailsService _emailsService;

    public RegistrationEmailsDataLoader(IBatchScheduler batchScheduler, EmailsService emailsService,
        DataLoaderOptions? options = null) : base(
        batchScheduler, options)
    {
        _emailsService = emailsService;
    }

    protected override async ValueTask FetchAsync(IReadOnlyList<Guid> keys, Memory<Result<Email[]>> results,
        CancellationToken cancellationToken)
    {
        var registrationEmails = await _emailsService.GetRegistrationsEmails(keys.ToArray());
        for (var i = 0; i < keys.Count; i++)
        {
            results.Span[i] = registrationEmails[keys[i]];
        }
    }
}