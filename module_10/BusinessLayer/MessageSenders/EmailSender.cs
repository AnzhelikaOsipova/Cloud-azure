using Models.Domain;
using System.Net;
using System.Net.Mail;

namespace BusinessLayer.MessageSenders
{
    public class EmailSender : IEmailSender
    {
        public Email LectorOfCourseEmail { get; }
        public string SmtpHost { get; }
        public string Password { get; }

        public EmailSender(Email lectorOfCourseEmail, string password, string smtpHost)
        {
            LectorOfCourseEmail = lectorOfCourseEmail;
            SmtpHost = smtpHost;
            Password = password;
        }

        public void Send(Email sendTo, string message)
        {
            MailAddress from = new MailAddress(LectorOfCourseEmail.CorrectEmail, "Lector");
            MailAddress to = new MailAddress(sendTo.CorrectEmail);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Notification";
            m.Body = message;
            m.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient(SmtpHost, 2525);
            smtp.Credentials = new NetworkCredential(LectorOfCourseEmail.CorrectEmail, Password);
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}
