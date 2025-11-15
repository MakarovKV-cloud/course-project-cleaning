using Cleaning.Data.Intefaces;
using Cleaning.Data.JsonStorage;
using Domain.CourseProjectCleaning;
using System.Collections.Generic;
using System.Windows;

namespace CourseProjectCleaning
{
    public partial class AdminManagementForm : Window
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRequestsRepository _requestsRepository;
        private List<Request> _requests;
        private List<User> _users;

        // Конструктор без параметров для совместимости
        public AdminManagementForm()
        {
            InitializeComponent();

            // Инициализация репозиториев (зависит от вашей архитектуры)
            _usersRepository = new UsersRepository(); // Замените на вашу реализацию
            _requestsRepository = new RequestsRepository(); // Замените на вашу реализацию

            LoadData();
        }

        // Конструктор с внедрением зависимостей (опционально)
        public AdminManagementForm(IUsersRepository usersRepository, IRequestsRepository requestsRepository)
        {
            InitializeComponent();
            _usersRepository = usersRepository;
            _requestsRepository = requestsRepository;
            LoadData();
        }

        private void LoadData()
        {
            ReadUsers(); // Теперь метод объявлен
            ReadRequests();
        }

        private void ReadUsers()
        {
            _users = _usersRepository.GetAll();
            // Привязка данных к DataGrid
            // dataGridUsers.ItemsSource = _users;
        }

        private void ReadRequests()
        {
            _requests = _requestsRepository.GetAllRequests();
            // Привязка данных к DataGrid
            // dataGridRequests.ItemsSource = _requests;
        }

        private void BtnAddCity_Click(object sender, RoutedEventArgs e)
        {
            // Добавление города
        }

        private void BtnRefreshData_Click(object sender, RoutedEventArgs e)
        {
            LoadData(); // Обновление всех данных
        }
    }
}