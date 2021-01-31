using System.IO;

namespace Identity.Api.Core
{
    public class Templates
    {

        //public class Email
        //{
        //    public static string ConfirmEmail(string userName, string url)
        //    {

        //        var emailMsg = File.ReadAllText("wwwroot/Templates/Email/SignUpVerification.html");
        //        emailMsg = emailMsg.Replace("[UserName]", userName);
        //        emailMsg = emailMsg.Replace("[URL]", url);
        //        emailMsg = emailMsg.Replace("[title]", Extensions.Title);
        //        emailMsg = emailMsg.Replace("[adminMail]", Extensions.AdminMail);
        //        emailMsg = emailMsg.Replace("[root]", Extensions.ApiUrl);
        //        //emailMsg = emailMsg.Replace("[fb]", CoreExtensions.Facebook);
        //        //emailMsg = emailMsg.Replace("[linkedIn]", CoreExtensions.LinkedIn);
        //        //emailMsg = emailMsg.Replace("[twitter]", CoreExtensions.Twitter);

        //        return emailMsg;
        //    }

        //    public static string ForgetPassword(string userName, string url)
        //    {
        //        var emailMsg = File.ReadAllText("wwwroot/Templates/Email/ForgetPassword.html");
        //        emailMsg = emailMsg.Replace("[UserName]", userName);
        //        emailMsg = emailMsg.Replace("[URL]", url);
        //        emailMsg = emailMsg.Replace("[title]", Extensions.Title);
        //        emailMsg = emailMsg.Replace("[adminMail]", Extensions.AdminMail);
        //        emailMsg = emailMsg.Replace("[root]", Extensions.ApiUrl);
        //        //emailMsg = emailMsg.Replace("[fb]", CoreExtensions.Facebook);
        //        //emailMsg = emailMsg.Replace("[linkedIn]", CoreExtensions.LinkedIn);
        //        //emailMsg = emailMsg.Replace("[twitter]", CoreExtensions.Twitter);

        //        return emailMsg;
        //    }

        //}
        public class Authentication
        {
            public static string RedirectToken(string token)
            {
                return $"<html>" +
                       $"<head>" +
                       @"<script type=""text/javascript"">" +
                       $"var token=\"{token}\"; " +
                       @"(function () {" +
                       $"window.opener.parent.postMessage(token,'{Extensions.AdminUrl}/login');" +
                       @"window.close();" +
                       @"})();" +
                       @"</script>" +
                       $"</head>" +
                       $"<body>" +
                       //$"<h1>Hello User</h1><p>{token}</p>" +
                       $"</body>" +
                       $"</html>";
            }
        }

        //public class Payment
        //{
        //    public static string Refund(string userName, string amount, string chargeId, string refundId, string status, string date)
        //    {
        //        var refundEmail = File.ReadAllText("wwwroot/Templates/Email/PaymentRefunded.html")
        //            .Replace("[Amount]", amount)
        //            .Replace("[ChargeID]", chargeId)
        //            .Replace("[RefundID]", refundId)
        //            .Replace("[Status]", status)
        //            .Replace("[UserName]", userName)
        //            .Replace("[title]", Extensions.Title)
        //            .Replace("[adminMail]", Extensions.AdminMail)
        //            .Replace("[root]", Extensions.ApiUrl)
        //            .Replace("[Date]", date);

        //        return refundEmail;
        //    }
        //    public static string SubscriptionConfirmation(string userName, string packageName)
        //    {
        //        var refundEmail = File.ReadAllText("wwwroot/Templates/Email/SubscriptionConfirmation.html")
        //            .Replace("[UserName]", userName)
        //            .Replace("[title]", Extensions.Title)
        //            .Replace("[adminMail]", Extensions.AdminMail)
        //            .Replace("[root]", Extensions.ApiUrl)
        //            .Replace("[PlanName]", packageName);
        //        return refundEmail;
        //    }
        //    public static string SubscriptionConfirmationInvoice(string userName, string packageName,string invoicePdf,string paylink)
        //    {
        //        var refundEmail = File.ReadAllText("wwwroot/Templates/Email/SubscriptionConfirmationInvoice.html")
        //            .Replace("[UserName]", userName)
        //            .Replace("[title]", Extensions.Title)
        //            .Replace("[adminMail]", Extensions.AdminMail)
        //            .Replace("[root]", Extensions.ApiUrl)
        //            .Replace("[PlanName]", packageName)
        //            .Replace("[Invoice]", invoicePdf)
        //            .Replace("[Paylink]", paylink);
        //        return refundEmail;
        //    }
        //}
    }
}



