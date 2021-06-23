using ETicket.Domain.Identity;
using ETicket.Repository.Implementation;
using ETicket.Repository.Interface;
using ETicket.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<ETicketAppUser> GetAllUsers()
        {
            var res = this.userRepository.GetAll();
            return res;
        }
    }
}
