using System;
using Mail.Api.Services;
using Microsoft.Extensions.Options;

namespace Mail.Api.Core
{
    public class GmailConfig: ISmtpConfig
    {
        public GmailConfig(IOptions<AppSettings> appSettings)
        {
            var settings = appSettings.Value.Gmail;
            if(settings==null)
                throw new Exception("Gmail configurations not found");

            Host = "smtp.gmail.com";
            Port = 465;
            UseSsl = true;
            Username = settings.Email;
            Password = settings.Password;
            Name = Extensions.ApplicationName;
            Email = settings.Email;
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class OutlookConfig : ISmtpConfig
    {
        public OutlookConfig(IOptions<AppSettings> appSettings)
        {
            var settings = appSettings.Value.Outlook;
            if (settings == null)
                throw new Exception("Outlook configurations not found");

            Host = "smtp.live.com";
            Port = 587;
            UseSsl = false;
            Username = settings.Email;
            Password = settings.Password;
            Name = Extensions.ApplicationName;
            Email = settings.Email;
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
