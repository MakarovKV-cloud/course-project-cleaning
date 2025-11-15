using System.Windows;
using System.Text.RegularExpressions;
using Cleaning.Data.Intefaces;

namespace CourseProjectCleaning
{

    public partial class RegistrationForm : Window
    {
        private readonly IUsersRepository _usersRepository;

        public RegistrationForm(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
            InitializeComponent();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            string lastName = txtLastName.Text;
            string firstName = txtFirstName.Text;
            string middleName = txtMiddleName.Text;
            string login = txtLogin.Text;
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            // Валидация данных
            if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            MessageBox.Show("Аккаунт успешно создан!", "Успех",
                          MessageBoxButton.OK, MessageBoxImage.Information);

            // Возврат к форме авторизации
            var authForm = new AuthorizationForm(_usersRepository);
            authForm.Show();
            this.Close();
        }
    }
}