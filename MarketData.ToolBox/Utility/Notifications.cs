using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.ToolBox.Utility
{
    public class Notifications
    {
        /// <summary>
        /// Send an email to the address specified for live trading notifications.
        /// </summary>
        /// <param name="subject">Subject of the email</param>
        /// <param name="message">Message body, up to 10kb</param>
        /// <param name="data">Data attachment (optional)</param>
        /// <param name="address">Email address to send to</param>
        public bool CustomEmail(string address, string subject, string message, string data = "")
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("admin.nick@bizcad.com");
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            MemoryStream stream = new MemoryStream();

            try
            {
                if (data.Length > 0)
                {
                    Attachment att = new Attachment(data.ToStream(), MediaTypeNames.Text.Plain);
                    msg.Attachments.Add(att);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.1and1.com";
            client.Port = 587;
            SecureString secure = new SecureString();
            string pwd = @"4w&A*X,7*";
            foreach (char t in pwd)
            {
                secure.AppendChar(t);
            }
            client.Credentials = new NetworkCredential("admin.nick@bizcad.com", secure);
            //client.EnableSsl = true;
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

    }
}
