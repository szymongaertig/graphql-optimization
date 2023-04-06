namespace Api.Model;

public class Ticket
{
    public Ticket(Registration registration)
    {
        Id = registration.Id;
        Status = registration.Status;
        RegistrationDate = registration.RegistrationDate;
    }
    
    public DateTimeOffset RegistrationDate { get; }
    public RegistrationStatus Status { get; }
    public Guid Id { get; }
    
    public string Name { get; }
}