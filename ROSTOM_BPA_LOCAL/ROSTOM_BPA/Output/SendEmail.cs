using System.Net.Mail;
using System.Net;

namespace ROSTOM_BPA_TOOLS.Output
{
    public class EmailSender
    {
        private string smtpServer;
        private int smtpPort;
        private string smtpUsername;
        private string smtpPassword;
        private bool enableSSL;

        public EmailSender(string server, int port, string username, string password, bool ssl)
        {
            smtpServer = server;
            smtpPort = port;
            smtpUsername = username;
            smtpPassword = password;
            enableSSL = ssl;
        }

        public void SendEmail(string from, string to, string subject, string body)
        {
            using (SmtpClient smtpClient = new SmtpClient(smtpServer))
            {
                smtpClient.Port = smtpPort;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = enableSSL;

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(from);
                    mail.To.Add(new MailAddress(to));
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    try
                    {
                        smtpClient.Send(mail);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in sending email: " + ex.Message);
                        
                    }
                }
            }
        }
    }
}
