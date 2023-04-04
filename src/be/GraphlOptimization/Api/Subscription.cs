using Api.Model;

namespace Api;

public class Subscription
{
    [Subscribe]
    [Topic("registration-created")]
    public Registration OnRegistered([EventMessage] Registration registration) => registration;

    [Subscribe]
    [Topic("registration-updated")]
    public Registration OnRegistrationUpdated([EventMessage] Registration registration) => registration;
}