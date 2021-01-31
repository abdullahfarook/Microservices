namespace Mail.Api.Core
{
    public static partial class Extensions
    {
        public static string ApplicationName = "Framework";

    }
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ApplicationName { get; set; }
        public EmailAuth Gmail { get; set; }
        public EmailAuth Outlook { get; set; }
    }

    public class EmailAuth
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
    }
}
