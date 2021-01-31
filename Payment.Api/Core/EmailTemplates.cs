using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microservices.Core;

namespace Payment.Api.Core
{
    public class SubscriptionConfirmation : IEmailArgument
    {
        public SubscriptionConfirmation(string userName, string url)
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
    public class PaymentRefunded : IEmailArgument
    {
        public PaymentRefunded(string userName, long amount, string chargeId, string refundId, string status, DateTime date)
        {
            UserName = userName;
            Amount = amount.ToString();
            ChargeId = chargeId;
            RefundId = refundId;
            Status = status;
            Date = date.ToShortTimeString();
        }

        public string Amount { get; set; }
        public string ChargeId { get; set; }
        public string RefundId { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public string Date { get; set; }
        public string Title { get; set; } = Extensions.ApplicationName;
        public string AdminMail { get; set; } = Extensions.AdminMail;
        public string Root { get; set; } = Extensions.ApiUrl;

    }

    public class SubscriptionCancelled : IEmailArgument
    {
        public SubscriptionCancelled()
        {
            throw new NotImplementedException();
        }
    }

    public class SubscriptionConfirmationInvoice : IEmailArgument
    {
        public SubscriptionConfirmationInvoice()
        {
            throw new NotImplementedException();
        }
    }

    public class SubscriptionTrialReminder : IEmailArgument
    {
        public SubscriptionTrialReminder()
        {
            throw new NotImplementedException();
        }
    }

    public class PaymentManualInvoice : IEmailArgument
    {
        public PaymentManualInvoice()
        {
            throw new NotImplementedException();
        }
    }

    public class PaymentFailure : IEmailArgument
    {
        public PaymentFailure()
        {
            throw new NotImplementedException();
        }
    }
}
