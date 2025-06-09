using Common.Constants;
using System;
using System.Configuration;
using System.Net.Mail;

namespace Notification
{
    public class EmailModule
    {
        //public string ToEmailAddress { get; set; }
        //public string CcEmail { get; set; }
        //public string MailBody { get; set; } 
        //public Attachment MailAttachment { get; set; }
        ////public string MailAttachment { get; set; }
        //public string MailSubject { get; set; }
        //public string MailFrom { get; set; }
        //public string MailFromPassword { get; set; }
      
        //public string Mailsignature { get; set; }
        //public  bool SendEmail()
        //{
        //    bool IsSent = false;
        //    string MailFrom = ConfigurationManager.AppSettings[WebConfigConstants.SMTP_EMAIL];
        //    string MailFromPassword = ConfigurationManager.AppSettings[WebConfigConstants.SMTP_EMAIL_PASSWORD];
        //    SmtpClient smtp = new SmtpClient();
        //    //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
        //    //smtp.UseDefaultCredentials = false;
        //    //smtp.Credentials = new NetworkCredential(MailFrom, MailFromPassword);
        //    MailMessage mailMessage = new MailMessage(MailFrom,ToEmailAddress);
        //    mailMessage.Subject = MailSubject;
        //    mailMessage.Body = MailBody;
        //    //if (CcEmail != null)
        //    if (!string.IsNullOrEmpty(CcEmail))
        //    {
        //        CcEmail = CcEmail.Trim();
        //        string[] CcEmails = CcEmail.Split(',');
        //        foreach (var email in CcEmails)
        //        {
        //            if (!string.IsNullOrEmpty(email))
        //            {
        //                mailMessage.CC.Add(email.Trim());
        //            }
        //        }
        //    }                        
        //    if (MailAttachment != null)
        //    { 
        //        mailMessage.Attachments.Add(MailAttachment);
        //    }
            
        //    mailMessage.IsBodyHtml = true;
        //    smtp.EnableSsl = true;
        //    try
        //    {
        //        smtp.Send(mailMessage);
        //        IsSent = true;

        //    }
        //    catch (Exception ex)
        //    {
               
        //        IsSent = false;
        //        throw ex;

        //    }
        //    return IsSent;
        //}
    }
}
