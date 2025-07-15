using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Entities;
using Test.Repo;

namespace Test.Services
{
    public class AdminUserService(AdminUserRepository _adminUserRepository)
    {
        public List<UserDetails> UserDetailsList()
        {
            return _adminUserRepository.UserDetailsList(); 
        }
        public string DeleteUser(int id)
        {
            return _adminUserRepository.DeleteUser(id);
        }

        public string Register(User user)
        {
            // call repo to add user
            _adminUserRepository.Register(user);
            return "User registered successfully";
        }

    }
}
