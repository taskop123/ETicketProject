using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Domain.Identity
{
    public class UserImportDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
