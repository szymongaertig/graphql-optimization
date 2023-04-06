namespace Api.Model;

public class Ticket
{
    public Ticket(Registration registration)
    {
        Id = registration.Id;
        Status = registration.Status;
        RegistrationDate = registration.RegistrationDate;
        ClientId = registration.ClientId;
    }
    public DateTimeOffset RegistrationDate { get; }
    public RegistrationStatus Status { get; }
    public Guid Id { get; }
    public Guid ClientId { get; set; }
}