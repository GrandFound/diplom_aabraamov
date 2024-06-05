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
    /// Логика взаимодействия для wAddClient.xaml
    /// </summary>
    public partial class wAddClient : Window
    {
        public wAddClient()
        {
            InitializeComponent();
            FIO.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
        }

        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[А-Яа-я]").Success) e.Handled = true;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Address.Text == "" || FIO.Text == "" || DateOfBirth.SelectedDate == null || Phone.Text == "")
                {
                    MessageBox.Show("Поля ФИО, телефона, даты рождения и адреса должны быть заполнены");
                }
                else
                {
                    var item = new Clients
                    {
                        FIO = FIO.Text,
                        DateOfBirth = DateOfBirth.SelectedDate.Value.Date,
                        Address = Address.Text,
                        PassportDetails = Passport.Text,
                        Phone = Phone.Text
                    };

                    GetContext().Clients.Add(item);
                    GetContext().SaveChanges();
                    MessageBox.Show("Клиент добавлен");
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
