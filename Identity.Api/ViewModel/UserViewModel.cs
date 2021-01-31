using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.ViewModel
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string FullName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public string Pic { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Address { get; set; }
        public virtual string Gender { get; set; }
        public virtual DateTimeOffset DateOfBirth { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string TimeZone { get; set; }
        public virtual int ZipCode { get; set; }
        public virtual string Country { get; set; }
        public bool IsBlocked { get; set; }
        public bool EmailConfirmed { get; set; }
        public virtual List<string> Roles { get; set; }
    }
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class RegisterViewModel: UserViewModel
    {
        [Required]
        public override string UserName { get; set; }
        [Required]
        public override string FirstName { get; set; }
        [Required]
        public override string LastName { get; set; }
        [Required]
        public override string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public override string Password { get; set; }

    }
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

    }
    public class UpdatePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
