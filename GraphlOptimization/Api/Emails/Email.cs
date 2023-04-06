namespace Api.Emails;

public class Email
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset? SentDate { get; set; }
}