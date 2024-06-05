using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для wEmployeePersonalArea.xaml
    /// </summary>
    public partial class wEmployeePersonalArea : Window
    {
        private static Users _currentUser = new Users();
        private static List<Employees> _listEmployees = new List<Employees>();
        private static Employees _currentEmployee = new Employees();
        private static List<Cars> _listCars = new List<Cars>();

        public wEmployeePersonalArea(Users current)
        {
            InitializeComponent();
            _currentUser = current;
            UpLoad();
        }

        private void UpLoad()
        {
            _listEmployees = GetContext().Employees.ToList();
            _currentEmployee = _listEmployees.Where(x => x.UserID == _currentUser.ID).First();
            DataContext = _currentEmployee;
            _listCars = GetContext().Cars.ToList();
            dgCars.ItemsSource = _listCars.Where(x => x.EmployeeID == _currentEmployee.ID);
        }

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
