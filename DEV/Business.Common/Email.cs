using Common.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Common
{
    public class Email
    {
        #region Properties
        //public string Sender { get; set; }
        //public List<string> Recipients { get; set; }
        //public string Subject { get; set; }
        //public string Body { get; set; }
        //public bool IsBodyHtml { get; set; }
        #endregion
        #region Constructors
        //public Email()
        //{
        //    Recipients = new List<string>();
        //    IsBodyHtml = true; // IsBodyHtml is set to true by default.
        //}
        #endregion

        #region Public Methods
        //public void Send()
        //{
        //    // Id the Sender is empty, then the system email is used.
        //    if (String.IsNullOrEmpty(Sender)) { Sender = ConfigurationManager.AppSettings[WebConfigConstants.SMTP_EMAIL]; }

        //    MailMessage message = new MailMessage()
        //    {
        //        From = new MailAddress(Sender),
        //        Subject = this.Subject,
        //        Body = this.Body,
        //        IsBodyHtml = this.IsBodyHtml
        //    };

        //    foreach (string recipient in Recipients)
        //    {
        //        message.To.Add(recipient);
        //    }

        //    SmtpClient smtp = new SmtpClient();
        //    //smtp.Timeout = 10000;
        //    //smtp.Host = ConfigurationManager.AppSettings[WebConfigConstants.SMTP_SERVER];
        //    //smtp.Port = 2525;
        //    //smtp.UseDefaultCredentials = false;
        //    //smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings[WebConfigConstants.SMTP_USER_NAME], ConfigurationManager.AppSettings[WebConfigConstants.SMTP_PASSWORD]);
        //    smtp.Send(message);

        //}
        #endregion
    }
}
