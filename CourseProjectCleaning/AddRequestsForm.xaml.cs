using System.Windows;
using System.Windows.Controls;

namespace CourseProjectCleaning
{
    public partial class AddRequestsForm : Window
    {
        public AddRequestsForm()
        {
            InitializeComponent();
            Loaded += AddRequestsForm_Loaded;
        }

        private void AddRequestsForm_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO: Загрузить города и услуги из базы данных
            LoadCities();
            LoadServices();
        }

        private void LoadCities()
        {
            // Временные данные
            cbCities.Items.Add(new { Id = 1, Name = "Москва" });
            cbCities.Items.Add(new { Id = 2, Name = "Санкт-Петербург" });
            cbCities.Items.Add(new { Id = 3, Name = "Новосибирск" });
        }

        private void LoadServices()
        {
            // Временные данные
            var services = new[]
            {
                new { Id = 1, Name = "Влажная уборка", Price = 50m, RequiresArea = true },
                new { Id = 2, Name = "Мытье окон", Price = 200m, RequiresArea = false },
                new { Id = 3, Name = "Химчистка ковров", Price = 150m, RequiresArea = true }
            };

            foreach (var service in services)
            {
                var checkBox = new CheckBox
                {
                    Content = $"{service.Name} - {service.Price:C}",
                    Tag = service,
                    Margin = new Thickness(0, 2, 0, 2)
                };
                checkBox.Checked += Service_CheckedChanged;
                checkBox.Unchecked += Service_CheckedChanged;
                spServices.Children.Add(checkBox);
            }
        }

        private void TxtArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateTotalCost();
        }

        private void Date_Changed(object sender, SelectionChangedEventArgs e)
        {
            // Можно добавить логику проверки даты
        }

        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Можно добавить логику в зависимости от города
        }

        private void Service_CheckedChanged(object sender, RoutedEventArgs e)
        {
            CalculateTotalCost();
        }

        private void CalculateTotalCost()
        {
            // TODO: Реализовать расчет стоимости
            tbTotalCost.Text = "2 500 руб.";
        }

        private void BtnProceedToPayment_Click(object sender, RoutedEventArgs e)
        {
            var paymentForm = new PaymentForm();
            paymentForm.Show();
            this.Close();
        }
    }
}