using ETicket.Domain;
using ETicket.Domain.DomainModels;
using ETicket.Service.Interface;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ETicket.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        public EmailService(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }
        public async Task SendEmailAsync(EmailMessage email)
        {
            MimeMessage emailMessage = new MimeMessage()
            {
                Sender = new MailboxAddress(this.emailSettings.SendersName, this.emailSettings.SmtpUserName),
                Subject = email.Subject
            };

            emailMessage.From.Add(new MailboxAddress(this.emailSettings.EmailDisplayName, this.emailSettings.SmtpUserName));
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = email.Content };
            emailMessage.To.Add(new MailboxAddress(email.MailTo));

            try
            {
                using(var stream = new MailKit.Net.Smtp.SmtpClient())
                {
                    var socketOption = this.emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
                    await stream.ConnectAsync(this.emailSettings.SmtpServer, this.emailSettings.SmtpServerPort, socketOption);
                    
                    if (!String.IsNullOrEmpty(this.emailSettings.SmtpUserName))
                    {
                        await stream.AuthenticateAsync(this.emailSettings.SmtpUserName, this.emailSettings.SmtpPassword);
                    }

                    await stream.SendAsync(emailMessage);

                    await stream.DisconnectAsync(true);

                }
            }
            catch(SmtpException ex)
            {
                throw ex;
            }

            throw new NotImplementedException();
        }
    }
}
