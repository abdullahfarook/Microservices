using System;

namespace Payment.Api.ViewModel
{
    public class ProrateViewModel
    {
        public int ChargeType { get; set; }
        public string PackageName { get; set; }
        public long Amount { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        
    }
}
