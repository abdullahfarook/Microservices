using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Api.ViewModel
{
    public class CustomerViewModel
    {
        public long Id { get; set; }
        public virtual string FullName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public string Pic { get; set; }
        public virtual string Email { get; set; }
    }
}
