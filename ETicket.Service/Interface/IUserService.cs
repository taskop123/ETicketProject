using ETicket.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Service.Interface
{
    public interface IUserService
    {
        IEnumerable<ETicketAppUser> GetAllUsers();
    }
}
