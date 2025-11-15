using Cleaning.Data.Intefaces;
using Domain.CourseProjectCleaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cleaning.Data.JsonStorage
{
    public class UsersRepository : IUsersRepository
    {
        private readonly List<User> _users = ReadUsers();

        public int Add(User user)
        {
            _users.Add(user);
            SaveUsers();
            return 0;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            return _users.ToList();
        }

        public bool Update(User user)
        {
            throw new NotImplementedException();
        }

        public bool Authenticate(string login, string password, string role)
        {
            return _users.Any(u => u.Login == login && u.Password == password && u.Role  == role);
        }

        private static List<User> ReadUsers()
        {
            try {
                var usersJson = File.ReadAllText("database-users.json");

                if (string.IsNullOrWhiteSpace(usersJson))
                {
                    return [];
                }

                return JsonSerializer.Deserialize<List<User>>(usersJson) ?? [];
            }
            catch
            {
                File.WriteAllText("database-users.json", "[]");
                return [];
            }
        }

        private void SaveUsers()
        {
            var usersJson = JsonSerializer.Serialize(_users);
            File.WriteAllText("database-users.json", usersJson);
        }
    }
}
