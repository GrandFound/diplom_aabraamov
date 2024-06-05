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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CarRental.AppData;

namespace CarRental
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Users currentUser = new Users();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentUser = GetContext().Users.Where(x => x.Login == tbLogin.Text && x.Password == pbPassword.Password).FirstOrDefault();
                if (currentUser == null)
                {
                    MessageBox.Show("Такого пользователя нет", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    switch (currentUser.RoleID)
                    {
                        case 1:
                            var wnd1 = new wAdministrator(currentUser)
                            {
                                Owner = this,
                                Title = "Окно администратора"
                            };
                            wnd1.Show();
                            Hide();
                            break;
                        case 2:
                            var wnd2 = new wEmployeePersonalArea(currentUser)
                            {
                                Owner = this,
                                Title = "Окно сотрудника"
                            };
                            wnd2.Show();
                            Hide();
                            break;
                        default:
                            MessageBox.Show("Данные не обнаружены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message.ToString() + "Критическая работа приложения", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
