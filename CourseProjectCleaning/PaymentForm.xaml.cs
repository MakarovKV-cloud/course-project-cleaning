using System.Windows;
using System.Windows.Controls;

namespace CourseProjectCleaning
{
    public partial class PaymentForm : Window
    {
        public PaymentForm()
        {
            InitializeComponent();
        }

        private void TxtCardNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Форматирование номера карты
        }

        private void TxtExpiryDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Форматирование срока действия
        }

        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Реализовать логику оплаты
            MessageBox.Show("Оплата прошла успешно!", "Успех",
                          MessageBoxButton.OK, MessageBoxImage.Information);

            var clientForm = new ClientViewForm();
            clientForm.Show();
            this.Close();
        }
    }
}