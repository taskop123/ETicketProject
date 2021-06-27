using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.Identity
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "This Field is Required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is Required!")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "This field is Required!")]
        public string ConfirmPassword { get; set; }
    }
}
