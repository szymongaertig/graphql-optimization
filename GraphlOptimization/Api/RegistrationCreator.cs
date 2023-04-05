using HotChocolate.Subscriptions;

namespace Api;

public class RegistrationCreator
{
    private readonly RegistrationsRepository _repository;
    private readonly ITopicEventSender _eventSender;

    public RegistrationCreator(RegistrationsRepository repository, ITopicEventSender eventSender)
    {
        _repository = repository;
        _eventSender = eventSender;
    }

    public async Task Create()
    {
        while (true)
        {
            var newRegistration = RegistrationsRepository.GetRandomRegistrations(1).First();
            _repository.CreateOrUpdateRegistration(newRegistration);
            _eventSender.SendAsync("registration-created", newRegistration);
            await Task.Delay(1000);
        }
    }
}