using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static CarRental.AppData;

namespace CarRental
{
    /// <summary>
    /// Логика взаимодействия для wRental.xaml
    /// </summary>
    public partial class wRental : Window
    {
        private static Cars _currentCar = new Cars();
        private static List<Cars> _listCar = new List<Cars>();
        private static Rental _currentRental = new Rental();
        private static Employees _currentEmployee = new Employees();
        private static List<Employees> _listEmployee = new List<Employees>();

        public wRental()
        {
            InitializeComponent();
            UpLoad();
            Events();
        }

        private void UpLoad()
        {
            _listCar = GetContext().Cars.Where(x => x.ReturnMark == true).ToList();
            _listEmployee = GetContext().Employees.ToList();
            DgCars.ItemsSource = _listCar;
            CbClient.ItemsSource = GetContext().Clients.ToList();
            CbClient.SelectedIndex = 0;

            ComboBoxItem item = new ComboBoxItem
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 0, 0, 1)
            };
            ComboBoxItem item1 = new ComboBoxItem
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 0, 0, 1)
            };
            CbSort.Items.Insert(0, "Сортировка");
            CbSort.Items.Add("Стоимость машины по возрастанию");
            CbSort.Items.Add("Стоимость машины по убыванию");
            CbSort.Items.Add(item);
            CbSort.Items.Add("Цена за день по возрастанию");
            CbSort.Items.Add("Цена за день по убыванию");
            CbSort.Items.Add(item1);
            CbSort.Items.Add("Год выпуска по возрастанию");
            CbSort.Items.Add("Год выпуска по убыванию");
            CbSort.SelectedIndex = 0;
        }

        private void UpdateCars()
        {
            _listCar = GetContext().Cars.Where(x => x.ReturnMark == true).ToList();
            if (CbSort.SelectedIndex > 0)
            {
                switch (CbSort.SelectedIndex)
                {
                    case 1:
                        _listCar = _listCar.OrderBy(p => p.CarPrice).ToList();
                        break;
                    case 2:
                        _listCar = _listCar.OrderByDescending(p => p.CarPrice).ToList();
                        break;
                    case 4:
                        _listCar = _listCar.OrderBy(p => p.RentalDayPrice).ToList();
                        break;
                    case 5:
                        _listCar = _listCar.OrderByDescending(p => p.RentalDayPrice).ToList();
                        break;
                    case 7:
                        _listCar = _listCar.OrderBy(p => p.YearOfIssue).ToList();
                        break;
                    case 8:
                        _listCar = _listCar.OrderByDescending(p => p.YearOfIssue).ToList();
                        break;
                }
            }
            DgCars.ItemsSource = _listCar;
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCars();
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DgCars.ItemsSource = _listCar.Where(Item => Item.RegistrationNumber.ToLower().Contains(TbSearch.Text.ToLower()) || Item.CarBrands.BrandName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void Events()
        {
            btnBack.Click += (s, e) =>
            {
                SwitchLayers(nameof(ChooseCar));
            };
        }

        private void SwitchLayers(string LayerName)
        {
            List<Grid> layers = new List<Grid>()
            {
                ChooseCar,
                RentalInfo
            };

            foreach (var layer in layers)
            {
                layer.Visibility = (layer.Name == LayerName) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void Get_Click(object sender, RoutedEventArgs e)
        {
            Cars current = (sender as Button)?.DataContext as Cars;
            _currentCar = current;
            DataContext = _currentCar;
            SwitchLayers(nameof(rental));
            SwitchLayers(nameof(RentalInfo));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            SwitchLayers(nameof(cars));
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentEmployee = _listEmployee.Where(x => x.FIO == tbEmployee.Text).First();
                int days = CountDays();
                int res = TotalCost(days);
                _currentRental.CarID = _currentCar.ID;
                _currentRental.DateOfIssue = DpDateOfIssue.SelectedDate.Value.Date;
                _currentRental.ReturnDate = DpReturnDate.SelectedDate.Value.Date;
                _currentRental.RentalPeriod = days;
                _currentRental.RentalCost = res;
                _currentRental.PaymentStamp = 0;
                _currentRental.ClientID = CbClient.SelectedIndex + 1;
                _currentRental.EmployeeID = _currentEmployee.ID;
                if (Service1.IsChecked == true)
                {
                    _currentRental.Service1ID = 1;
                }
                if (Service2.IsChecked == true)
                {
                    _currentRental.Service2ID = 2;
                }
                if (Service3.IsChecked == true)
                {
                    _currentRental.Service3ID = 3;
                }

                GetContext().Rental.Add(_currentRental);
                GetContext().SaveChanges();
                MessageBox.Show("Прокат оформлен");
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
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
                DateTime begin = DpDateOfIssue.SelectedDate.Value.Date;
                DateTime end = DpReturnDate.SelectedDate.Value.Date;
                if(begin < DateTime.Today)
                {
                    MessageBox.Show("Дата начала проката не может быть меньше сегодняшней даты");
                    return 0;
                }
                else
                {
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
    }
}
