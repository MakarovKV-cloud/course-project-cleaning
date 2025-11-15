using Cleaning.Data.Intefaces;
using Cleaning.Data.JsonStorage;
using Domain.CourseProjectCleaning;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CourseProjectCleaning
{
    public partial class AdminManagementForm : Window
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRequestsRepository _requestsRepository;
        private readonly ICitiesRepository _citiesRepository;
        private readonly IServicesRepository _servicesRepository;

        private List<User> _users = new List<User>();
        private List<Request> _requests = new List<Request>();
        private List<City> _cities = new List<City>();
        private List<Service> _services = new List<Service>();

        public AdminManagementForm()
        {
            InitializeComponent();

            // Инициализация репозиториев
            _usersRepository = new UsersRepository();
            _requestsRepository = new RequestsRepository();
            _citiesRepository = new CitiesRepository();
            _servicesRepository = new ServicesRepository();

            LoadData();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Инициализация комбобоксов
            statusComboBox.ItemsSource = new List<string>
            {
                "Новая", "В работе", "Выполнена", "Отменена"
            };

            // Инициализация годов для статистики
            var currentYear = DateTime.Now.Year;
            var years = Enumerable.Range(currentYear - 2, 5).ToList();
            cbStatsYear.ItemsSource = years;
            cbStatsYear.SelectedItem = currentYear;
        }

        private void LoadData()
        {
            try
            {
                _users = _usersRepository.GetAll();
                _requests = _requestsRepository.GetAllRequests();
                _cities = _citiesRepository.GetAll();
                _services = _servicesRepository.GetAll();

                // Обновление UI с данными
                usersDataGrid.ItemsSource = _users;
                requestsDataGrid.ItemsSource = _requests;
                dgCities.ItemsSource = _cities;
                dgServices.ItemsSource = _services;

                UpdateStatistics();
                UpdateCharts();
                UpdateCityStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                // Обновление статистики
                totalUsersTextBlock.Text = $"Пользователей: {_users.Count}";
                totalRequestsTextBlock.Text = $"Заявок: {_requests.Count}";

                var newRequests = _requests.Count(r => r.Status == "Новая");
                newRequestsTextBlock.Text = $"Новых: {newRequests}";

                var totalIncome = _requests.Where(r => r.Status == "Выполнена").Sum(r => r.TotalCost);
                totalIncomeTextBlock.Text = $"Доход: {totalIncome:C}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статистики: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCharts()
        {
            try
            {
                var selectedYear = cbStatsYear.SelectedItem as int? ?? DateTime.Now.Year;
                var monthlyStats = CalculateMonthlyStatistics(selectedYear);

                // Обновляем таблицу статистики
                dgMonthlyStats.ItemsSource = monthlyStats;

                // Создаем графики с OxyPlot
                UpdateRequestsChart(monthlyStats);
                UpdateIncomeChart(monthlyStats);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении диаграмм: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateRequestsChart(List<MonthlyChartData> monthlyStats)
        {
            var plotModel = new PlotModel
            {
                Title = "Количество заявок по месяцам",
                TitleFontSize = 14,
                TitleFontWeight = OxyPlot.FontWeights.Bold
            };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Месяцы",
                FontSize = 10
            };

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Количество заявок",
                FontSize = 10,
                Minimum = 0
            };

            var columnSeries = new ColumnSeries
            {
                Title = "Заявки",
                FillColor = OxyColors.LightBlue,
                StrokeColor = OxyColors.DarkBlue,
                StrokeThickness = 1
            };

            foreach (var stat in monthlyStats)
            {
                categoryAxis.Labels.Add(stat.MonthName);
                columnSeries.Items.Add(new ColumnItem(stat.RequestsCount));
            }

            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);
            plotModel.Series.Add(columnSeries);

            requestsChart.Model = plotModel;
        }

        private void UpdateIncomeChart(List<MonthlyChartData> monthlyStats)
        {
            var plotModel = new PlotModel
            {
                Title = "Доходы по месяцам",
                TitleFontSize = 14,
                TitleFontWeight = OxyPlot.FontWeights.Bold
            };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Месяцы",
                FontSize = 10
            };

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Доход, руб.",
                FontSize = 10,
                Minimum = 0,
                StringFormat = "C0"
            };

            var lineSeries = new LineSeries
            {
                Title = "Доход",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Red,
                MarkerFill = OxyColors.White,
                Color = OxyColors.Blue,
                StrokeThickness = 2
            };

            foreach (var stat in monthlyStats)
            {
                categoryAxis.Labels.Add(stat.MonthName);
                lineSeries.Points.Add(new DataPoint(categoryAxis.Labels.Count - 1, (double)stat.TotalIncome));
            }

            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);
            plotModel.Series.Add(lineSeries);

            incomeChart.Model = plotModel;
        }

        private void UpdateCityStatistics()
        {
            try
            {
                var cityStats = _cities.Select(city => new CityStatistic(
                    city.Name ?? "Неизвестно",
                    _requests.Count(r => r.CityId == city.Id),
                    _requests.Where(r => r.CityId == city.Id && r.Status == "Выполнена").Sum(r => r.TotalCost)
                )).ToList();

                dgCityStats.ItemsSource = cityStats;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статистики по городам: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<MonthlyChartData> CalculateMonthlyStatistics(int year)
        {
            var monthlyStats = new List<MonthlyChartData>();
            var monthNames = new[] { "Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек" };

            for (int month = 1; month <= 12; month++)
            {
                var monthRequests = _requests.Where(r =>
                    r.CreatedAt.Year == year &&
                    r.CreatedAt.Month == month).ToList();

                var completedRequests = monthRequests.Where(r => r.Status == "Выполнена").ToList();
                var totalIncome = completedRequests.Sum(r => r.TotalCost);
                var averageIncome = completedRequests.Any() ? totalIncome / completedRequests.Count : 0;

                monthlyStats.Add(new MonthlyChartData(
                    year,
                    month,
                    monthNames[month - 1],
                    monthRequests.Count,
                    totalIncome,
                    averageIncome
                ));
            }

            return monthlyStats;
        }

        private void StatsYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCharts();
        }

        private void BtnAddCity_Click(object sender, RoutedEventArgs e)
        {
            string newCity = txtNewCity.Text.Trim();
            if (!string.IsNullOrWhiteSpace(newCity))
            {
                try
                {
                    var city = new City { Name = newCity };
                    _citiesRepository.Add(city);
                    MessageBox.Show($"Город '{newCity}' добавлен", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    txtNewCity.Clear();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении города: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите название города", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newUser = new User
                {
                    Name = "Новый пользователь",
                    Login = $"user{DateTime.Now.Ticks}",
                    Password = "password",
                    Role = "Client"
                };

                _usersRepository.Add(newUser);
                MessageBox.Show("Пользователь успешно добавлен", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (usersDataGrid.SelectedItem is User selectedUser)
            {
                try
                {
                    var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя {selectedUser.Name}?",
                                               "Подтверждение удаления",
                                               MessageBoxButton.YesNo,
                                               MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        if (_usersRepository.Delete(selectedUser.Id))
                        {
                            MessageBox.Show("Пользователь успешно удален", "Успех",
                                          MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить пользователя", "Ошибка",
                                          MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnUpdateRequestStatus_Click(object sender, RoutedEventArgs e)
        {
            if (requestsDataGrid.SelectedItem is Request selectedRequest &&
                statusComboBox.SelectedItem != null)
            {
                try
                {
                    string newStatus = statusComboBox.SelectedItem.ToString() ?? "Новая";
                    if (_requestsRepository.UpdateStatus(selectedRequest.Id, newStatus))
                    {
                        MessageBox.Show("Статус заявки успешно обновлен", "Успех",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось обновить статус заявки", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении статуса: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку и статус", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnAssignCleaner_Click(object sender, RoutedEventArgs e)
        {
            if (requestsDataGrid.SelectedItem is Request selectedRequest)
            {
                try
                {
                    var cleaners = _usersRepository.GetAll().Where(u => u.Role == "Cleaner").ToList();

                    if (cleaners.Any())
                    {
                        var cleaner = cleaners.First();
                        if (_requestsRepository.AssignCleaner(selectedRequest.Id, cleaner.Id))
                        {
                            MessageBox.Show($"Клинер {cleaner.Name} назначен на заявку", "Успех",
                                          MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нет доступных клинеров", "Информация",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при назначении клинера: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для назначения клинера", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnExportData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var exportData = $"Отчет от {DateTime.Now:dd.MM.yyyy HH:mm}\n" +
                               $"Пользователей: {_users.Count}\n" +
                               $"Заявок: {_requests.Count}\n" +
                               $"Новых заявок: {_requests.Count(r => r.Status == "Новая")}\n" +
                               $"Завершенных заявок: {_requests.Count(r => r.Status == "Выполнена")}\n" +
                               $"Общий доход: {_requests.Where(r => r.Status == "Выполнена").Sum(r => r.TotalCost):C}";

                System.IO.File.WriteAllText($"report_{DateTime.Now:yyyyMMdd_HHmmss}.txt", exportData);

                MessageBox.Show("Данные экспортированы в файл", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usersDataGrid.SelectedItem is User selectedUser)
            {
                selectedUserInfoTextBlock.Text = $"Выбран: {selectedUser.Name} ({selectedUser.Role})";
            }
        }

        private void RequestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (requestsDataGrid.SelectedItem is Request selectedRequest)
            {
                selectedRequestInfoTextBlock.Text = $"Выбрана заявка #{selectedRequest.Id} - {selectedRequest.Status}";
            }
        }
    }

    // Вспомогательные классы для данных
    public class MonthlyChartData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int RequestsCount { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal AverageIncome { get; set; }

        public MonthlyChartData(int year, int month, string monthName, int requestsCount, decimal totalIncome, decimal averageIncome)
        {
            Year = year;
            Month = month;
            MonthName = monthName;
            RequestsCount = requestsCount;
            TotalIncome = totalIncome;
            AverageIncome = averageIncome;
        }
    }

    public class CityStatistic
    {
        public string CityName { get; set; }
        public int RequestsCount { get; set; }
        public decimal TotalIncome { get; set; }

        public CityStatistic(string cityName, int requestsCount, decimal totalIncome)
        {
            CityName = cityName;
            RequestsCount = requestsCount;
            TotalIncome = totalIncome;
        }
    }
}