using ETicket.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ETicket.Domain.DTO
{
    public class AddUserToRole
    {
        [Display(Name = "User Emails")]
        public List<ETicketAppUser> UserEmails{ get; set; }
        [Display(Name = "Active Roles")]
        public List<string> Roles { get; set; }
        [Display(Name = "User Email")]
        public string SelectedUserId { get; set; }
        [Display(Name = "Role")]
        public string SelectedRole { get; set; }
        public AddUserToRole()
        {
            this.UserEmails = new List<ETicketAppUser>();
            this.Roles = new List<string>();
        }
    }
}
