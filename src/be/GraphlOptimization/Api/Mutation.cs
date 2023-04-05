using Api.Model;
using AutoFixture;
using HotChocolate.Subscriptions;

namespace Api;

public class Mutation
{
    private readonly RegistrationsRepository _registrationsRepository;

    public Mutation(RegistrationsRepository registrationsRepository)
    {
        _registrationsRepository = registrationsRepository;
    }

    public async Task<Registration?> CheckIn(Guid registrationId,
        [Service] ITopicEventSender eventSender,
        CancellationToken cancellationToken)
    {
        await Task.Delay(2000);
       var existingRegistration = _registrationsRepository.FindRegistrationById(registrationId);
        if (existingRegistration == null)
        {
            throw new Exception("Could not find registration");
        }

        if (existingRegistration.CheckInDate != null)
        {
            throw new Exception("Registration has already been checked-in");
        }

        existingRegistration.CheckInDate = DateTimeOffset.Now;
        _registrationsRepository.CreateOrUpdateRegistration(existingRegistration);
        await eventSender.SendAsync("registration-updated", existingRegistration, cancellationToken);
        return existingRegistration;
    }

    public async Task<Registration?> CreateRandomRegistration([Service] ITopicEventSender eventSender,
        CancellationToken cancellationToken)
    {
        var newRegistration = RegistrationsRepository.GetRandomRegistrations(1)
            .First();
        _registrationsRepository.CreateOrUpdateRegistration(newRegistration);
        await eventSender.SendAsync("registration-created", newRegistration, cancellationToken);
        return newRegistration;
    }
}