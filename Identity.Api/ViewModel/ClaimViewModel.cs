using System.Collections.Generic;

namespace Identity.Api.ViewModel
{
    public class PermissionGroup
    {
        public PermissionGroup()
        {
            Permissions = new List<ClaimViewModel>();
        }
        public string Name { get; set; }
        public List<ClaimViewModel> Permissions { get; set; }
        
    }

    public class ClaimViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
