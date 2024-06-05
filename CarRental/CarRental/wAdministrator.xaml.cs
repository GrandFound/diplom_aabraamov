using System;
using System.Collections;
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
    /// Логика взаимодействия для wAdministrator.xaml
    /// </summary>
    public partial class wAdministrator : Window
    {
        private static Users _currentUser = new Users();
        private static Employees _currentEmployee = new Employees();
        private static List<Cars> _listCars = new List<Cars>();
        private static List<Rental> _listRental = new List<Rental>();
        private static List<Clients> _listClients = new List<Clients>();
        private static List<Employees> _listEmployees = new List<Employees>();
        private static List<string> _listFilterStatus = new List<string>();

        public wAdministrator(Users current)
        {
            InitializeComponent();
            _currentUser = current;
            UpLoad();
            Events();
        }

        private void UpLoad()
        {
            _currentEmployee = GetContext().Employees.Where(x => x.UserID == _currentUser.ID).First();
            DataContext = _currentEmployee;
            _listRental = GetContext().Rental.ToList();
            _listCars = GetContext().Cars.ToList();
            _listClients = GetContext().Clients.ToList();
            _listEmployees = GetContext().Employees.Where(x => x.ID != _currentEmployee.ID).ToList();
            dgCars.ItemsSource = _listCars;
            dgClients.ItemsSource = _listClients;
            dgEmployees.ItemsSource = _listEmployees;
            dgRentalHistory.ItemsSource = _listRental.OrderByDescending(x => x.DateOfIssue).ToList();

            _listFilterStatus = new List<string>();
            _listFilterStatus.Insert(0, "Фильтрация");
            _listFilterStatus.Add("В прокате");
            _listFilterStatus.Add("Свободно");
            CbFilter.ItemsSource = _listFilterStatus;

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
            if (CbSort.Items.Count == 0)
            {
                CbSort.Items.Insert(0, "Сортировка");
                CbSort.Items.Add("Стоимость машины по возрастанию");
                CbSort.Items.Add("Стоимость машины по убыванию");
                CbSort.Items.Add(item);
                CbSort.Items.Add("Цена за день по возрастанию");
                CbSort.Items.Add("Цена за день по убыванию");
                CbSort.Items.Add(item1);
                CbSort.Items.Add("Год выпуска по возрастанию");
                CbSort.Items.Add("Год выпуска по убыванию");
            }
            CbSort.SelectedIndex = 0;
            CbFilter.SelectedIndex = 0;
        }

        // Cars
        #region
        private void UpdateCars()
        {
            _listCars = GetContext().Cars.ToList();
            if (CbSort.SelectedIndex > 0)
            {
                switch (CbSort.SelectedIndex)
                {
                    case 1:
                        _listCars = _listCars.OrderBy(p => p.CarPrice).ToList();
                        break;
                    case 2:
                        _listCars = _listCars.OrderByDescending(p => p.CarPrice).ToList();
                        break;
                    case 4:
                        _listCars = _listCars.OrderBy(p => p.RentalDayPrice).ToList();
                        break;
                    case 5:
                        _listCars = _listCars.OrderByDescending(p => p.RentalDayPrice).ToList();
                        break;
                    case 7:
                        _listCars = _listCars.OrderBy(p => p.YearOfIssue).ToList();
                        break;
                    case 8:
                        _listCars = _listCars.OrderByDescending(p => p.YearOfIssue).ToList();
                        break;
                }
            }
            if (CbFilter.SelectedIndex > 0)
            {
                switch (CbFilter.SelectedIndex)
                {
                    case 1:
                        _listCars = _listCars.Where(p => p.Status == "В прокате").ToList();
                        break;
                    case 2:
                        _listCars = _listCars.Where(p => p.Status == "Свободен").ToList();
                        break;
                }
            }
            dgCars.ItemsSource = _listCars;
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCars();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCars();
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                dgCars.ItemsSource = _listCars.Where(Item => Item.RegistrationNumber.ToLower().Contains(TbSearch.Text.ToLower()) || Item.CarBrands.BrandName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void EditCar_Click(object sender, RoutedEventArgs e)
        {
            Cars current = (sender as Button)?.DataContext as Cars;
            var wnd = new wEditCar(current);
            wnd.ShowDialog();
            UpLoad();
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new wAddCar();
            wnd.ShowDialog();
            UpLoad();
        }
        #endregion

        // Rental
        #region
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            start.SelectedDate = null;
            end.SelectedDate = null;
            _listRental = GetContext().Rental.OrderByDescending(x => x.DateOfIssue).ToList();
            dgRentalHistory.ItemsSource = _listRental.ToList();
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            _listRental = GetContext().Rental.OrderByDescending(x => x.DateOfIssue).ToList();
            if (start.SelectedDate != null && end.SelectedDate != null)
            {
                if (start.SelectedDate > end.SelectedDate)
                {
                    MessageBox.Show("Дата начала должна быть меньше даты конца");
                }
                else
                {
                    _listRental = _listRental.Where(x => x.DateOfIssue >= start.SelectedDate && x.ReturnDate <= end.SelectedDate ||
                    x.DateOfIssue <= start.SelectedDate && x.ReturnDate >= start.SelectedDate ||
                    x.DateOfIssue >= start.SelectedDate && x.DateOfIssue <= end.SelectedDate ||
                    x.DateOfIssue <= start.SelectedDate && x.ReturnDate >= end.SelectedDate).ToList();
                    dgRentalHistory.ItemsSource = _listRental;
                }
            }
            else
            {
                MessageBox.Show("Заполните оба поля даты");
            }
        }

        private void AddRental_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new wRental();
            wnd.ShowDialog();
            UpLoad();
            start.SelectedDate = null;
            end.SelectedDate = null;
        }

        private void EditRental_Click(object sender, RoutedEventArgs e)
        {
            Rental current = (sender as Button)?.DataContext as Rental;
            var editRentalWindow = new wEditRental(current);
            editRentalWindow.ShowDialog();
            UpLoad();
            start.SelectedDate = null;
            end.SelectedDate = null;
        }
        #endregion

        // Layers
        #region
        private void Events()
        {
            Rental.Click += (s, e) =>
            {
                SwitchLayers(nameof(RentalHistory));
            };

            Cars.Click += (s, e) =>
            {
                SwitchLayers(nameof(AllCars));
            };

            Clients.Click += (s, e) =>
            {
                SwitchLayers(nameof(AllClients));
            };

            Employees.Click += (s, e) =>
            {
                SwitchLayers(nameof(AllEmployees));
            };
        }

        private void SwitchLayers(string LayerName)
        {
            List<Grid> layers = new List<Grid>()
            {
                RentalHistory,
                AllCars,
                AllClients,
                AllEmployees
            };

            foreach (var layer in layers)
            {
                layer.Visibility = (layer.Name == LayerName) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void Rental_Click(object sender, RoutedEventArgs e)
        {
            SwitchLayers(nameof(rentalHistory));
        }

        private void Cars_Click(object sender, RoutedEventArgs e)
        {
            SwitchLayers(nameof(cars));
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            SwitchLayers(nameof(clients));
        }

        private void Employees_Click(object sender, RoutedEventArgs e)
        {
            SwitchLayers(nameof(employees));
        }

        #endregion

        // Clients
        #region
        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            var addClientWindow = new wAddClient();
            addClientWindow.ShowDialog();
            UpLoad();
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            Clients current = (sender as Button)?.DataContext as Clients;
            var editClientWindow = new wEditClient(current);
            editClientWindow.ShowDialog();
            UpLoad();
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            Clients currentClient = (sender as Button)?.DataContext as Clients;

            var clientForRemoving = GetContext().Clients.Where(p => p.ID == currentClient.ID).First();

            if (MessageBox.Show($"Вы точно хотите удалить клиента?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    GetContext().Clients.Remove(clientForRemoving);
                    GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void TbSearchClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                dgClients.ItemsSource = _listClients.Where(Item => Item.FIO.ToLower().Contains(TbSearchClient.Text.ToLower()) || Item.Address.ToLower().Contains(TbSearchClient.Text.ToLower())).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }
        #endregion

        // Employees
        #region
        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employees current = (sender as Button)?.DataContext as Employees;
            var newRentalWindow = new wEditEmployee(current);
            newRentalWindow.ShowDialog();
            UpLoad();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new wAddEmployee();
            addEmployeeWindow.ShowDialog();
            UpLoad();
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employees currentEmployee = (sender as Button)?.DataContext as Employees;

            var employeeForRemoving = GetContext().Employees.Where(p => p.ID == currentEmployee.ID).First();
            var userForRemoving = GetContext().Users.Where(p => p.ID == employeeForRemoving.UserID).First();

            if (MessageBox.Show($"Вы точно хотите удалить сотрудника?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    GetContext().Employees.Remove(employeeForRemoving);
                    GetContext().Users.Remove(userForRemoving);
                    GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void TbSearchEmployee_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                dgEmployees.ItemsSource = _listEmployees.Where(Item => Item.FIO.ToLower().Contains(TbSearchEmployee.Text.ToLower()) || Item.Positions.Position_name.ToLower().Contains(TbSearchEmployee.Text.ToLower())).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }
        #endregion

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new MainWindow();
            wnd.Show();
            Hide();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new wEditEmployee(_currentEmployee);
            editWindow.ShowDialog();
            UpLoad();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Закрыть программу?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
