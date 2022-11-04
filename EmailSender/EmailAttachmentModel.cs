using Microsoft.AspNetCore.Http;

namespace EmailSender;

public class EmailAttachmentModel
{
    public IFormFile File { get; set; }
    public string FileName { get; set; }
}