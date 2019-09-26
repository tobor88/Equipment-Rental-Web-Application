using System;
using System.Net.Mail;

namespace CheckOutForm
{
    public class SendEmail
    {
        static void SmtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            _ = e.UserState;
        }
        public void SendMail(string toAddress, string fromAddress, string subject, string message)
        {
            try
            {
                using var mail = new MailMessage
                {
                    From = new MailAddress(fromAddress)
                };
                mail.To.Add(new MailAddress(toAddress));
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = false;

                try
                {
                    // If you dont have an SMTP2GO account get one so yu dont have to store clear text credentials on one of your servers.
                    using var smtpClient = new SmtpClient("smtp2go.com", 2525)
                    {
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    smtpClient.Send(mail);
                    smtpClient.SendCompleted += new SendCompletedEventHandler(SmtpClient_SendCompleted);
                    smtpClient.SendAsync(mail, new object());
                }
                finally
                {
                    mail.Dispose();
                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Console.WriteLine("Failed to deliver message to {0}",
                            ex.InnerExceptions[i].FailedRecipient);
                    }
                }
            }
        }
    }
}
