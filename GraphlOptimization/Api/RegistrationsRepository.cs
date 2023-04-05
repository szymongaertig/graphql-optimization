using Api.Model;
using AutoFixture;

namespace Api;

public class RegistrationsRepository
{
    private IList<Registration> _source = new List<Registration>();
    public RegistrationsRepository()
    {
        var registrations = GetRandomRegistrations(new[]
        {
            Guid.Parse("2fb0dd27-5954-4f89-ad28-3689a794922b"),
            Guid.Parse("5cd77eee-c9ab-463d-b2f7-fe9eae07dda6"),
            Guid.Parse("acb3f799-65be-406e-9da9-8aafc7164443"),
            Guid.Parse("acb3f799-65be-406e-9da9-8aafc7164443"),
            Guid.Parse("4c1f774c-26d9-4f59-b663-693c02b68638"),
            Guid.Parse("d4f330fe-1308-4f9d-84a7-2431169fffba"),
        });
        foreach (var registration in registrations)
        {
            _source.Add(registration);
        }
    }

    public static  Registration[] GetRandomRegistrations(Guid[] ids)
    {
        return ids.Select(id => new Fixture()
            .Build<Registration>()
            .With(x => x.Id, id)
            .With(x => x.CheckInDate, value: null)
            .With(x => x.RegistrationDate, DateTimeOffset.Now)
            .With(x => x.Status, RegistrationStatus.WaitingForPayment)
            .Create()).ToArray();    
    }
    
    public static  Registration[] GetRandomRegistrations(int numberOfRegistrations)
    {
        return new Fixture()
            .Build<Registration>()
            .With(x => x.CheckInDate, value: null)
            .With(x => x.RegistrationDate, DateTimeOffset.Now)
            .With(x => x.Status, RegistrationStatus.WaitingForPayment)
            .CreateMany(numberOfRegistrations)
            .ToArray();
    }
    public Registration[] GetRegistrations()
    {
        return _source
            .OrderByDescending(x => x.RegistrationDate)
            .ToArray();
    }

    public void CreateOrUpdateRegistration(Registration registration)
    {
        if (registration == null)
            throw new InvalidOperationException();
        
        var fromExisting = _source.FirstOrDefault(x => x.Id == registration.Id);
        if (fromExisting == null)
        {
            _source.Add(registration);
        }
        else
        {
            fromExisting.Name = registration.Name;
            fromExisting.Surname = registration.Name;
            fromExisting.CheckInDate = registration.CheckInDate;
        }
    }

    public Registration? FindRegistrationById(Guid registrationId)
    {
        return _source.FirstOrDefault(x => x.Id == registrationId);
    }
}