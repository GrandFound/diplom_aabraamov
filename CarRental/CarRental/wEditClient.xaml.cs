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
    /// Логика взаимодействия для wEditClient.xaml
    /// </summary>
    public partial class wEditClient : Window
    {
        private static Clients _currentUser = new Clients();

        public wEditClient(Clients current)
        {
            InitializeComponent();
            _currentUser = current;
            UpLoad();
        }

        private void UpLoad()
        {
            DataContext = _currentUser;
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
                var item = _currentUser;
                item.FIO = FIO.Text;
                item.DateOfBirth = DateOfBirth.SelectedDate.Value.Date;
                item.Address = Address.Text;
                item.PassportDetails = Passport.Text;
                item.Phone = Phone.Text;

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
