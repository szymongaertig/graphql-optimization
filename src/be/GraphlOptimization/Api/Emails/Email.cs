namespace Api.Emails;

public class Email
{
    public string Subject { get; set; }
    public string Title { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset? SentDate { get; set; }
}