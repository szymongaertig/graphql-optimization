using Api.Model;
using HotChocolate.Caching;

namespace Api;

public class Query
{
    private readonly RegistrationsRepository _registrationsRepository;

    public Query(RegistrationsRepository registrationsRepository)
    {
        _registrationsRepository = registrationsRepository;
    }
    
    public async Task<Registration[]> GetEventRegistrations()
    {
        return _registrationsRepository.GetRegistrations();
    }
    
    [CacheControl(2_000)]
    public async Task<Ticket> GetTicket(Guid ticketId)
    {
        await Task.Delay(2000);
        var registration = _registrationsRepository.FindRegistrationById(ticketId);
        if (registration == null)
        {
            throw new Exception("Unknown ticketId");
        }
        return new Ticket(registration);
    }
}