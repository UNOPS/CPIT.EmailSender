# Email sender

It is a .Net library which helps you to send emails from your .Net project

## Before You Begin

Currently, the library is using [Mailjet](https://www.mailjet.com) to send emails. You need to have an active Mailjet account to use this library.

## How To Use
#### Step 1: Adding the required appsettings
You should have a "MailJetSettings" section in appsettings.json file, which contains three main configurations:
- mailJetApiKey
- mailJetApiSecret
- senderEmail

#### Step 2: Configure dependency injection
Add the following code in your Startup file
```
services.AddScoped<EmailSenderManager>();
```

#### Step 3: Use email sender service
- Inject the class EmailSenderManager in your controller
- Create cshtml email template in your assembly
- Call sending email function
```
await _emailSender.SendEmail(new EmailModel
        {
            TemplateName = {FULL_Template_Name_Including_Namespace},
            Title = {TITLE},
            Attachments = {ATTACHMENTS},
            EmailReceivers = {[RECEIVERS_EMAILS]}
        }, model);
``` 