using Cleaning.Data.Intefaces;
using System.Windows;

namespace CourseProjectCleaning
{

    public partial class App : Application
    {
        private IUsersRepository _usersRepository;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _usersRepository = new Cleaning.Data.JsonStorage.UsersRepository();

            var authForm = new AuthorizationForm(_usersRepository);
            authForm.Show();
        }
    }
}