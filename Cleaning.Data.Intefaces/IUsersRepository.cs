using Domain.CourseProjectCleaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleaning.Data.Intefaces
{
    public interface IUsersRepository
    {
        int Add(User user);
        List<User> GetAll();
        bool Update(User user);
        bool Delete(int id);
        bool Authenticate(string login, string password, string role);
    }
}
