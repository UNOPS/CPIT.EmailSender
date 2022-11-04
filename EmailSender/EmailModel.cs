namespace EmailSender;

public class EmailModel
{
    public string TemplateName { get; set; }
    public string Title { get; set; }
    public List<EmailAttachmentModel>? Attachments { get; set; }
    public string[] EmailReceivers { get; set; }
}