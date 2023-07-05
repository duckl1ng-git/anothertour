using MimeKit;
using MailKit.Net.Smtp;

namespace anothertour
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            MimeMessage msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Другая Эксурсия", "somemailbox@anothertour.ru"));
            msg.To.Add(new MailboxAddress("", email));
            msg.Subject = subject;
            msg.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message + "<br /><br /> С уважением,<br />Другая Экскурсия" };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("anothertour.ru", 465);
                //Bad practice
                await smtp.AuthenticateAsync("somemailbox@anothertour.ru", "password");
                await smtp.SendAsync(msg);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}