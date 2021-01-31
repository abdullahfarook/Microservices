using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.Api.ViewModel
{
    public class CardViewModel
    {
        public virtual string Id { get; set; }
        public virtual string Token { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Last4 { get; set; }
        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }
        public bool IsDefault { get; set; }
        public DateTime Created { get; set; }
        public string AddressCity { get; set; }
        public string Country { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressZip { get; set; }
    }
    public class CreateCardViewModel :CardViewModel
    {
        [Required]
        public override string Token { get; set; }
    }

    public class UpdateCardViewModewl : CardViewModel
    {
        [Required]
        public override string Id { get; set; }
    }
}
