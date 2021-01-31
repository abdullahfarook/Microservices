using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Core
{
    public interface IEmailArgument
    {

    }
    public interface IEmailSender
    {
        //Task<(bool success, string errorMsg)> SendEmailAsync(dynamic sender, dynamic[] recepients, string subject, string body, bool isHtml = true);
        //Task<(bool success, string errorMsg)> SendEmailAsync(string senderName, string senderEmail, string recepientName, string recepientEmail, string subject, string body, bool isHtml = true);
        void SendEmail(string recepientName, string recepientEmail, string subject, IEmailArgument arguments, bool isHtml = true);

    }
}
