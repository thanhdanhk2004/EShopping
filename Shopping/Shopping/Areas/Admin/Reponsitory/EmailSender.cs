using System.Net;
using System.Net.Mail;

namespace Shopping.Areas.Admin.Reponsitory
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("b@gmail.com", "") 
            };
            return client.SendMailAsync(
                new MailMessage(from: "b@gmail.com",
                                to:email,
                                subject,
                                message));
        }
    }
}
