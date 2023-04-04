using Api.Model;
using AutoFixture;

namespace Api;

public class RegistrationsRepository
{
    private IList<Registration> _source = new List<Registration>();
    public RegistrationsRepository()
    {
        var registrations = GetRandomRegistrations(10);
        foreach (var registration in registrations)
        {
            _source.Add(registration);
        }
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
    
    public void AddRegistration(Registration registration)
    {
        _source.Add(registration);
    }
}