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
                Credentials = new NetworkCredential("thanhdan27102004sayhi@gmail.com", "yscanlpfrcixttnt") 
            };
            return client.SendMailAsync(
                new MailMessage(from: "thanhdan27102004sayhi@gmail.com",
                                to:email,
                                subject,
                                message));
        }
    }
}
