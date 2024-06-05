using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static CarRental.AppData;

namespace CarRental
{
    /// <summary>
    /// Логика взаимодействия для wEditRental.xaml
    /// </summary>
    public partial class wEditRental : Window
    {
        private static Cars _currentCar = new Cars();
        private static List<Cars> _listCar = new List<Cars>();
        private static List<Employees> _listEmployee = new List<Employees>();
        private static Employees _currentEmployee = new Employees();
        private static Rental _current;

        public wEditRental(Rental current)
        {
            InitializeComponent();
            _current = current;
            UpLoad();
        }

        public void UpLoad()
        {
            DataContext = _current;
            _listCar = GetContext().Cars.ToList();
            _listEmployee = GetContext().Employees.ToList();
            _currentCar = _listCar.Where(x => x.ID == _current.CarID).First();
            CbClient.ItemsSource = GetContext().Clients.ToList();
            CbClient.SelectedIndex = 0;
            Payment_stamp.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            if (_current.Service1ID != null) Service1.IsChecked = true;
            if (_current.Service2ID != null) Service2.IsChecked = true;
            if (_current.Service3ID != null) Service3.IsChecked = true;
        }

        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[0-9.]").Success) e.Handled = true;
        }

        private void Count_Click(object sender, RoutedEventArgs e)
        {
            int days = CountDays();
            int res = TotalCost(days);
            Counted.Content = res.ToString();
            Period.Content = days.ToString();
        }
        public int CountDays()
        {
            int days;
            try
            {
                DateTime begin = DateTime.Parse(DpDateOfIssue.Text);
                DateTime end = DateTime.Parse(DpReturnDate.Text);
                if (begin > end)
                {
                    MessageBox.Show("Дата начала проката должна быть меньше даты окончания");
                    return 0;
                }
                else
                {
                    TimeSpan difference = end.Subtract(begin);
                    days = Convert.ToInt32(difference.TotalDays) + 1;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Выберите даты начала и окончания проката");
                return 0;
            }
            return days;
        }

        public int TotalCost(int days)
        {
            int res;
            int costService = 0;
            int oneday = _currentCar.RentalDayPrice;
            if (Service1.IsChecked == true)
            {
                costService += 500;
            }
            if (Service2.IsChecked == true)
            {
                costService += 3000;
            }
            if (Service3.IsChecked == true)
            {
                costService += 1000;
            }
            res = oneday * Convert.ToInt32(days) + costService;
            return res;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int days = CountDays();
                int res = TotalCost(days);
                var item = _current;
                item.CarID = _currentCar.ID;
                item.DateOfIssue = DateTime.Parse(DpDateOfIssue.Text);
                item.ReturnDate = DateTime.Parse(DpReturnDate.Text);
                item.RentalPeriod = days;
                item.RentalCost = res;
                item.PaymentStamp = Convert.ToInt32(Payment_stamp.Text);
                item.ClientID = CbClient.SelectedIndex + 1;
                if (Service1.IsChecked == true)
                {
                    item.Service1ID = 1;
                }
                if (Service2.IsChecked == true)
                {
                    item.Service2ID = 2;
                }
                if (Service3.IsChecked == true)
                {
                    item.Service3ID = 3;
                }

                GetContext().SaveChanges();
                Close();
            }
            catch
            {
                MessageBox.Show("Введите корректный формат");
            }
            Hide();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var rentalForRemoving = GetContext().Rental.Where(p => p.ID == _current.ID).ToList();
            if (MessageBox.Show($"Вы точно хотите удалить запись об этом прокате?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    GetContext().Rental.RemoveRange(rentalForRemoving);
                    GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                    Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
