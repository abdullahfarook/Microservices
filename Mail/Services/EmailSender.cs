using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microservices.Core;
using MimeKit;

namespace Mail.Api.Services
{
    public interface IEmailSender
    {
        Task<ApiResult> SendEmailAsync(MailboxAddress sender, MailboxAddress[] recepients, string subject, string body, bool isHtml = true);
        Task<ApiResult> SendEmailAsync(string recepientName, string recepientEmail, string subject, string body, bool isHtml = true);
        Task<ApiResult> SendEmailAsync(string senderName, string senderEmail, string recepientName, string recepientEmail, string subject, string body,bool isHtml = true);
    }

    public class EmailSender : IEmailSender
    {
        private ISmtpConfig _config;


        public EmailSender(ISmtpConfig config)
        {
            _config = config;
        }


        public async Task<ApiResult> SendEmailAsync(
            string recepientName,
            string recepientEmail,
            string subject,
            string body,
            bool isHtml = true)
        {
            var from = new MailboxAddress(_config.Name, _config.Email);
            var to = new MailboxAddress(recepientName, recepientEmail);

            return await SendEmailAsync(from, new[] { to }, subject, body,isHtml);
        }



        public async Task<ApiResult> SendEmailAsync(
            string senderName,
            string senderEmail,
            string recepientName,
            string recepientEmail,
            string subject,
            string body,
            bool isHtml = true)
        {
            var from = new MailboxAddress(senderName, senderEmail);
            var to = new MailboxAddress(recepientName, recepientEmail);

            return await SendEmailAsync(from, new[] { to }, subject, body, isHtml);
        }



        public async Task<ApiResult> SendEmailAsync(
            MailboxAddress sender,
            MailboxAddress[] recepients,
            string subject,
            string body,
            bool isHtml = true)
        {
            MimeMessage message = new MimeMessage();

            message.From.Add(sender);
            message.To.AddRange(recepients);
            message.Subject = subject;
            message.Body = isHtml ? new BodyBuilder { HtmlBody = body }.ToMessageBody() : new TextPart("plain") { Text = body };

            try
            {
                if (_config == null) throw new Exception("Email Configurations Not Found");

                using (var client = new SmtpClient())
                {
                    if (!_config.UseSsl)
                        client.ServerCertificateValidationCallback = (sender2, certificate, chain, sslPolicyErrors) => true;

                    await client.ConnectAsync(_config.Host, _config.Port, _config.UseSsl).ConfigureAwait(false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    if (!string.IsNullOrWhiteSpace(_config.Username))
                        await client.AuthenticateAsync(_config.Username, _config.Password).ConfigureAwait(false);

                    await client.SendAsync(message).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }

                return ApiResult.Success();
            }
            catch (Exception ex)
            {
                 return ApiResult.Failed(ex);
            }
        }
    }

    public interface ISmtpConfig
    {
         string Host { get; set; }
         int Port { get; set; }
         bool UseSsl { get; set; }

         string Name { get; set; }
         string Username { get; set; }
         string Email { get; set; }
         string Password { get; set; }
    }
}
