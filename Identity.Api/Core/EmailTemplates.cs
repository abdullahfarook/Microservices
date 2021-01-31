using Microservices.Core;

namespace Identity.Api.Core
{
    public class ForgetPassword : IEmailArgument
    {
        public ForgetPassword(string userName, string url)
        {
            UserName = userName;
            Url = url;
        }
        public string UserName { get; set; }
        public string Url { get; set; }
        public string Title { get; set; } = Extensions.ApplicationName;
        public string AdminMail { get; set; } = Extensions.AdminMail;
        public string Root { get; set; } = Extensions.ApiUrl;
    }

    public class SignUpVerification : IEmailArgument
    {
        public SignUpVerification(string userName, string url)
        {
            UserName = userName;
            Url = url;
        }
        public string UserName { get; set; }
        public string Url { get; set; }
        public string Title { get; set; } = Extensions.ApplicationName;
        public string AdminMail { get; set; } = Extensions.AdminMail;
        public string Root { get; set; } = Extensions.ApiUrl;
    }
}
