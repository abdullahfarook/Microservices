using System;

namespace Payment.Api.ViewModel
{
    public class ChargeViewModel
    {
       public string Id { get; set; }
       public long Amount { get; set; }
       public DateTime Created { get; set; }
       public bool Refunded { get; set; }
    }
}
