namespace Api.Model;

public class Registration
{
    public Guid Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string EmailAddress { get; set; }
    public DateTimeOffset RegistrationDate { get; set; }
    public DateTimeOffset? CheckInDate { get; set; }
    public Guid EventId { get; set; }
    public RegistrationStatus Status { get; set; }
}

public enum RegistrationStatus
{
    WaitingForPayment,
    Completed
}