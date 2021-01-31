using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.DataAccess.Model
{
    public class User : IdentityUser<long>
    {
        [Column("UserID")]
        public override long Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string? Pic { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
        public int ZipCode { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LatestUpdatedOn { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public bool IsBlocked { get; set; }
        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<IdentityUserRole<long>> Roles { get; set; }

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<long>> UserClaims { get; set; }
    }
}
