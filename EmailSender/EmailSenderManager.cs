using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EmailSender;

public class EmailSenderManager
{
    private readonly MailjetClient mailjetClient;
    private readonly IConfiguration configuration;

    public EmailSenderManager(IConfiguration configuration)
    {
        var mailjetSettings = configuration.GetSection("MailJetSettings");

        if (mailjetSettings.GetSection("mailJetApiKey").Value != null)
        {
            mailjetClient = new MailjetClient(
                mailjetSettings.GetSection("mailJetApiKey").Value,
                mailjetSettings.GetSection("mailJetApiSecret").Value);
        }
      
        this.configuration = configuration;
    }

    public MailjetRequest ToMailJetMessage(string title, string email, string[] toAddresses,
        List<EmailAttachmentModel>? files)
    {
        var mailjetSettings = configuration.GetSection("MailJetSettings");
        var attachments = new JArray();
        if (files != null)
        {
            foreach (var fileModel in files)
                using (var ms = new MemoryStream())
                {
                    fileModel.File.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    var fileData = Convert.ToBase64String(fileBytes);
                    attachments.Add(
                        new JObject
                        {
                        {"Content-type", fileModel.File.ContentType},
                        {"Filename", fileModel.FileName},
                        {"content", fileData}
                        });
                }
        }

        var request = new MailjetRequest
            {
                Resource = Send.Resource
            }
            .Property(Send.FromEmail, mailjetSettings.GetSection("senderEmail").Value)
            .Property(Send.Subject, title)
            .Property(Send.HtmlPart, email)
            .Property(Send.To, string.Join(",", toAddresses))
            .Property(Send.Attachments, attachments);

        return request;
    }

    public async Task SendEmail<T>(EmailModel email, T model)
    {
        if (email.EmailReceivers.Any())
        {
            if (mailjetClient == null)
            {
                throw new ApplicationException("Please make sure to add the mailjet configuration to appsettings.json file");
            }
            var emailBody = ViewRenderer.RenderPartialView(email.TemplateName, model);
            var mailJetRequest = ToMailJetMessage(email.Title, emailBody, email.EmailReceivers, email.Attachments);

            var response = await mailjetClient.PostAsync(mailJetRequest);
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Sending email failed due to {response.GetErrorMessage()}");
        }
    }
}