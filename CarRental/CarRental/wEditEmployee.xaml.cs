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
    /// Логика взаимодействия для wEditEmployee.xaml
    /// </summary>
    public partial class wEditEmployee : Window
    {
        private static Employees _currentEmployee = new Employees();
        private static Users _currentUser = new Users();
        private static Positions _currentPosition = new Positions();
        private static List<Positions> _positions = new List<Positions>();

        public wEditEmployee(Employees current)
        {
            InitializeComponent();
            _currentEmployee = current;
            UpLoad();
        }

        private void UpLoad()
        {
            DataContext = _currentEmployee;
            _positions = GetContext().Positions.ToList();
            _currentPosition = _positions.Where(x => x.ID == _currentEmployee.PositionID).First();
            _currentUser = GetContext().Users.Where(x => x.ID == _currentEmployee.UserID).First();
            CbPositions.ItemsSource = _positions;
            CbPositions.SelectedIndex = _currentPosition.ID -1;
            if (_currentEmployee.Gender == "М")
                Male.IsChecked = true;
            else
                Female.IsChecked = true;
            FIO.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
        }

        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[А-Яа-я]").Success) e.Handled = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = _currentEmployee;
                item.FIO = FIO.Text;
                item.DateOfBirth = DateOfBirth.SelectedDate.Value.Date;
                if (Female.IsChecked == true)
                {
                    item.Gender = "Ж";
                }
                else
                {
                    item.Gender = "М";
                }
                item.Address = Address.Text;
                item.PassportDetails = Passport.Text;
                item.PositionID = CbPositions.SelectedIndex + 1;

                var item1 = _currentUser;
                item1.Login = tbLogin.Text;
                item1.Password = tbPassword.Text;

                GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }
    }
}
