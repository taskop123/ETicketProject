using ETicket.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<ETicketAppUser> GetAll();
        ETicketAppUser Get(string id);
        void Insert(ETicketAppUser entity);
        void Update(ETicketAppUser entity);
        void Delete(ETicketAppUser entity);
    }
}
