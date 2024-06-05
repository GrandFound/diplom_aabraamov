using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для wAddEmployee.xaml
    /// </summary>
    public partial class wAddEmployee : Window
    {
        private static List<Positions> _listPositions = new List<Positions>();

        public wAddEmployee()
        {
            InitializeComponent();
            UpLoad();
        }

        private void UpLoad()
        {
            FIO.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            _listPositions = GetContext().Positions.ToList();
            CbPositions.ItemsSource = _listPositions;
        }

        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[А-Яа-я]").Success) e.Handled = true;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FIO.Text == "" || DateOfBirth.SelectedDate == null || Passport.Text == "" || tbLogin.Text == "" || tbPassword.Text == "")
                {
                    MessageBox.Show("Поля ФИО, паспорта, даты рождения, логина и пароля должны быть заполнены");
                }
                else
                {
                    var item1 = new Users
                    {
                        Login = tbLogin.Text,
                        Password = tbPassword.Text
                    };
                    if (CbPositions.SelectedIndex == 1) item1.RoleID = 1;
                    else item1.RoleID = 2;

                    GetContext().Users.Add(item1);
                    GetContext().SaveChanges();

                    var item = new Employees
                    {
                        FIO = FIO.Text,
                        DateOfBirth = DateOfBirth.SelectedDate.Value.Date,
                        Address = Address.Text,
                        PassportDetails = Passport.Text,
                        PositionID = CbPositions.SelectedIndex = 1,
                        UserID = item1.ID
                    };
                    if (Female.IsChecked == true)
                    {
                        item.Gender = "Ж";
                    }
                    else
                    {
                        item.Gender = "М";
                    }

                    GetContext().Employees.Add(item);
                    GetContext().SaveChanges();
                    MessageBox.Show("Сотрудник добавлен");
                    Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
