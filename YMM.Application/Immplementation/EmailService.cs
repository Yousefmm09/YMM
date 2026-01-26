using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Auth;
namespace YMM.Application.Immplementation
{
    public class EmailService:IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<string> SendEmail(string email, string message)
        {
            using (var send = new SmtpClient())
            {
                using var client = new SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync("yousefmohsen232@gmail.com", "qgpydigixhizwkey");

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = message,
                    TextBody = "FaceBookClone"
                };

                var mail = new MimeMessage()
                {
                    Body = bodyBuilder.ToMessageBody(),
                    Subject = "Facebook Clone"
                };

                mail.From.Add(new MailboxAddress("YMM Company", "yousefmohsen232@gmail.com"));
                mail.To.Add(new MailboxAddress("", email));
                mail.Subject = message;
                mail.Body = bodyBuilder.ToMessageBody();

                await client.SendAsync(mail);
                await client.DisconnectAsync(true);
            }
            return "Email Sent Successfully";
        }
   

            public async Task SendResetPasswordEmailAsync(
                string toEmail,
                string userName,
                string resetUrl)
            {
                var message = new MimeMessage();
                message.From.Add(
                    new MailboxAddress("YMM Company", _settings.From)
                );
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = "Reset Your Password";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = BuildResetPasswordTemplate(userName, resetUrl),
                    TextBody = "Reset your password"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _settings.Host,
                    _settings.Port,
                    SecureSocketOptions.SslOnConnect
                );

                await client.AuthenticateAsync(
                    _settings.UserName,
                    _settings.Password
                );

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            private string BuildResetPasswordTemplate(string userName, string resetUrl)
            {
                return $@"
            <h2>Password Reset Request</h2>

            <p>Hello {userName},</p>

            <p>You requested to reset your password.</p>

            <a href='{resetUrl}'
               style='padding:12px 20px;
                      background:#1e90ff;
                      color:white;
                      text-decoration:none;
                      border-radius:6px;
                      display:inline-block;'>
               Reset Password
            </a>

            <p>This link expires in 15 minutes.</p>
            <p>If you didn’t request this, please ignore this email.</p>";
            }
        }
    }
