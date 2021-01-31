using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.DataAccess.Model
{
    public class Role : IdentityRole<long>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Role"/>.
        /// </summary>
        /// <remarks>
        /// The EventId property is initialized to from a new GUID string value.
        /// </remarks>
        public Role()
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="Role"/>.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <remarks>
        /// The EventId property is initialized to from a new GUID string value.
        /// </remarks>
        public Role(string roleName) : base(roleName)
        {

        }


        /// <summary>
        /// Initializes a new instance of <see cref="Role"/>.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <param name="description">Description of the role.</param>
        /// <remarks>
        /// The EventId property is initialized to from a new GUID string value.
        /// </remarks>
        public Role(string roleName, string description) : base(roleName)
        {
            Description = description;
        }
        [Column("RoleID")]
        public override long Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public virtual ICollection<IdentityRoleClaim<long>> RoleClaims { get; set; }
        public virtual ICollection<IdentityUserRole<long>> Users { get; set; }
    }
}
