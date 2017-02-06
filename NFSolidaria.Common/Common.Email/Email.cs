using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using Common.Domain.Interfaces;

namespace Common.Email
{
    public class Email : IEmail
    {
        private MailMessage mailMessage;
        public Email()
        {
            this.mailMessage = new MailMessage();
        }
        public string Subject { get; set; }
        public string Message { get; set; }
        public void EmailRecipientsAdd(string EmailRecipient)
        {
            this.mailMessage.To.Add(EmailRecipient);
        }
        public void EmailRecipientsClear()
        {
            this.mailMessage.To.Clear();
        }

        public bool Send()
        {

            var senha = ConfigurationManager.AppSettings["SmtpPassword"];
            var user = ConfigurationManager.AppSettings["SmtpUser"];
            var SMTP = ConfigurationManager.AppSettings["SmtpHost"];
            var port = ConfigurationManager.AppSettings["SmtpPort"];
            var emailSender = ConfigurationManager.AppSettings["EmailSender"];
            var nameSender = ConfigurationManager.AppSettings["NameSender"];
            var enableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["enableSSL"]);

            string assuntoMensagem = Subject;
            string conteudoMensagem = Message;

            this.mailMessage.ReplyTo = new System.Net.Mail.MailAddress(emailSender);
            this.mailMessage.From = new System.Net.Mail.MailAddress(nameSender + "<" + emailSender + ">");
            this.mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            this.mailMessage.IsBodyHtml = true;
            this.mailMessage.Subject = assuntoMensagem;
            this.mailMessage.Body = conteudoMensagem;

            this.mailMessage.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            this.mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

            smtp.Credentials = new System.Net.NetworkCredential(user, senha);
            smtp.Host = SMTP;
            smtp.Port = port.IsNullOrEmpty() ? 25 : Convert.ToInt32(port) ;
            smtp.EnableSsl = enableSSL;

            try
            {
                smtp.Send(this.mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        
        }


        public void Dispose()
        {
            this.mailMessage.Dispose();
        }
    }
}
