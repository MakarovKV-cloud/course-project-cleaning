using Domain.CourseProjectCleaning;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CourseProjectCleaning
{
    public partial class ClientViewForm : Window
    {
        public ClientViewForm()
        {
            InitializeComponent();
            Loaded += ClientViewForm_Loaded;
        }

        private void ClientViewForm_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserRequests();
        }

        private void LoadUserRequests()
        {
            var requests = new List<Request>
            {
                new Request { Id = 1, Status = "Новая", CleaningDate = DateTime.Now.AddDays(2),
                            Address = "ул. Ленина, д. 10, кв. 25", TotalCost = 2500m, CleanerName = "Не назначен" },
                new Request { Id = 2, Status = "В работе", CleaningDate = DateTime.Now.AddDays(-1),
                            Address = "пр. Мира, д. 45, кв. 12", TotalCost = 1800m, CleanerName = "Иванов А.С." },
                new Request { Id = 3, Status = "Завершена", CleaningDate = DateTime.Now.AddDays(-5),
                            Address = "ул. Центральная, д. 3, кв. 7", TotalCost = 3200m, CleanerName = "Петрова М.И." }
            };

            dgRequests.ItemsSource = requests;
        }

        private void BtnCreateRequest_Click(object sender, RoutedEventArgs e)
        {
            var addRequestForm = new AddRequestsForm();
            addRequestForm.Show();
            this.Close();
        }

        private void BtnDeleteRequest_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequests.SelectedItem is Request selectedRequest)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите отменить заявку #{selectedRequest.Id}?",
                                           "Отмена заявки", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show($"Заявка #{selectedRequest.Id} отменена",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUserRequests();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заявку для отмены",
                              "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DgRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDeleteRequest.IsEnabled = dgRequests.SelectedItem != null;
        }
    }


}