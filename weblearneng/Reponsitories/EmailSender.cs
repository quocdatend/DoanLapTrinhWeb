using System.Net;
using System.Net.Mail;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("hnqdat2003@gmail.com", "yoav fpqr pjwe uagk")
        };

        return client.SendMailAsync(
            new MailMessage(from: "hnqdat2003@gmail.com",
                            to: email,
                            subject,
                            message
                            ));
    }
}