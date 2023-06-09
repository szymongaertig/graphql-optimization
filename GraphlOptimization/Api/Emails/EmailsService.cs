using AutoFixture;
using Serilog;

namespace Api.Emails;

public class EmailsService
{
    private static Email[] _source = new Fixture()
        .CreateMany<Email>(10).ToArray()
        .ToArray();
    
    public async Task<Email[]> GetRegistrationEmails(Guid registrationId, int? takeLast = null)
    {
        await Task.Delay(1000);
        Log.Information("Invocation of GetRegistrationEmails for {RegistrationId}",registrationId);
        return _source.Take(takeLast.HasValue ? takeLast.Value : _source.Length).ToArray();
    }

    public async Task<IDictionary<Guid, Email[]>> GetRegistrationsEmails(Guid[] registrationIds)
    {
        await Task.Delay(1000);
        Log.Information("Invocation of GetRegistrationsEmails for {registrationIds}",registrationIds);
        return registrationIds.ToDictionary(x => x, x => _source);
    }
}