using Microsoft.Extensions.Options;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

public class Mailer
{
    private IOptions<MailSettings> _mailSettings;



    public string fromMail { get; set; }
    public string Password { get; set; }
    public string displayName { get; set; }


    public string hostName { get; set; }
    public int portNumber { get; set; }
    public bool enableSSL { get; set; }
    public bool issecured { get; set; }

    public string adminEmail { get; set; }


    public Mailer(IOptions<MailSettings> MailSettings)
    {

        _mailSettings = MailSettings;
        fromMail = _mailSettings.Value.fromMail;
        Password = _mailSettings.Value.Password;
        displayName = _mailSettings.Value.displayName;
        adminEmail = _mailSettings.Value.adminEmail;

        hostName = _mailSettings.Value.hostName;
        portNumber = _mailSettings.Value.portNumber;
        enableSSL = _mailSettings.Value.enableSSL;
        issecured = _mailSettings.Value.issecured;

    }
    public Mailer()
    {
    }

    public bool SendMail(string toEmails, string subject, string body, bool isHtml, string[] attachments,
            bool handleException)
    {
        bool retValue = false;
        try
        {

            //var client = new SmtpClient();
            var mail = new MailMessage();
            foreach (string toEmail in toEmails.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                mail.To.Add(toEmail.Trim());
            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = body;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = isHtml;
            mail.Priority = MailPriority.Normal;
            mail.From = new MailAddress(fromMail, displayName);
            //MailAddress copy = new MailAddress(adminEmail);
            // mail.Bcc.Add(copy);

            //var inlineLogo =
            //    new Attachment(ConfigurationManager.AppSettings["EmailLogo"]);



            //mail.Attachments.Add(inlineLogo);
            //const string contentId = "Image";
            //inlineLogo.ContentId = contentId;

            ////To make the image display as inline and not as attachment

            //inlineLogo.ContentDisposition.Inline = true;
            //inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

            ////To embed image in email
            //if (mail.Body != null)
            //    mail.Body = mail.Body.Replace("{LOGOIMAGE}", " <img src=\"cid:" + contentId + "\">");

            //if (attachments != null)
            //{
            //    foreach (string filePath in attachments)
            //    {

            //        var mailAttachment = new Attachment(filePath);
            //        mail.Attachments.Add(mailAttachment);
            //    }
            //}

            LogginLib.WriteLog("---mailer code worked--");
            LogginLib.WriteLog(portNumber.ToString());
            // client.Send(mail);
            // smtp settings
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = portNumber;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromMail, Password);
                smtp.Timeout = 20000;


            }



            smtp.Send(mail);
            // Passing values to smtp object
            //smtp.Send("themoviemb@gmail.com", toEmails, subject, body);

            retValue = true;
        }
        catch (Exception ex)
        {
            retValue = false;
            LogginLib.WriteLog("Mail Exception");

            LogginLib.WriteLog(ex);

            LogginLib.WriteLog("Mail Exception");
        }
        return retValue;
    }


    public bool SendMail_TLS(string toEmails, string subject, string body, bool isHtml, string[] attachments,
           bool handleException)
    {
        bool retValue = false;
        try
        {

            //var client = new SmtpClient();
            var mail = new MailMessage();
            foreach (string toEmail in toEmails.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                mail.To.Add(toEmail.Trim());
            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = body;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = isHtml;
            mail.Priority = MailPriority.Normal;
            mail.From = new MailAddress(fromMail, displayName);
            MailAddress copy = new MailAddress(adminEmail);
            mail.Bcc.Add(copy);

            //var inlineLogo =
            //    new Attachment(ConfigurationManager.AppSettings["EmailLogo"]);



            //mail.Attachments.Add(inlineLogo);
            //const string contentId = "Image";
            //inlineLogo.ContentId = contentId;

            ////To make the image display as inline and not as attachment

            //inlineLogo.ContentDisposition.Inline = true;
            //inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

            ////To embed image in email
            //if (mail.Body != null)
            //    mail.Body = mail.Body.Replace("{LOGOIMAGE}", " <img src=\"cid:" + contentId + "\">");

            //if (attachments != null)
            //{
            //    foreach (string filePath in attachments)
            //    {

            //        var mailAttachment = new Attachment(filePath);
            //        mail.Attachments.Add(mailAttachment);
            //    }
            //}

      
            var smtp = new System.Net.Mail.SmtpClient();

            if (issecured)
            {

                LogginLib.WriteLog("Secured");
                smtp.Host = hostName;
                smtp.Port = portNumber;
                smtp.EnableSsl = enableSSL;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromMail, Password);
                smtp.Timeout = 20000;


            }
            else
            {

                LogginLib.WriteLog("Not Secured");
                smtp.Host = hostName;
                smtp.Port = portNumber;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Timeout = 20000;


            }


            smtp.Send(mail);

            // Passing values to smtp object
            //smtp.Send("themoviemb@gmail.com", toEmails, subject, body);

            retValue = true;
        }
        catch (Exception ex)
        {
            // LogginLib.WriteLog(ex);

            LogginLib.WriteLog("SendMail Exception Message: " + ex.Message);
            if (ex.InnerException != null)
                LogginLib.WriteLog(" SendMail Exception Inner:   " + ex.InnerException);


        }
        LogginLib.WriteLog("---mailer code worked--");
        return retValue;
    }
    


}


