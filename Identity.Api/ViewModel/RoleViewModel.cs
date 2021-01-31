using System.Collections.Generic;

namespace Identity.Api.ViewModel
{
    public class RoleViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ClaimViewModel> Claims { get; set; }
    }
}
