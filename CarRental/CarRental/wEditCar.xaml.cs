using Microsoft.Win32;
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
    /// Логика взаимодействия для wEditCar.xaml
    /// </summary>
    public partial class wEditCar : Window
    {
        private static Cars _current;
        private string DirAuto = @"Resources\img\emptycar.png";
        private string Dir1 = "";
        private static string Dir;

        public wEditCar(Cars current)
        {
            InitializeComponent();
            _current = current;
            UpLoad();
        }

        public void UpLoad()
        {
            DataContext = _current;
            CbEmployee.ItemsSource = GetContext().Employees.ToList();
            YearOfIssue.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            CarPrice.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            RentalDayPrice.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
        }

        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[0-9.]").Success) e.Handled = true;
        }

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog { };
            try
            {
                if (file.ShowDialog() == true)
                {
                    string filename = file.FileName;
                    Dir1 = filename;
                    addPhotoButton.Content = filename;
                }
                else
                {
                    MessageBox.Show("Вы не выбрали файл.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dir1 = _current.Image;
                if (Dir1 == "") Dir = DirAuto;
                else Dir = Dir1;

                var item = _current;
                item.RegistrationNumber = RegistrationNumber.Text;
                item.BodyNumber = BodyNumber.Text;
                item.EngineNumber = EngineNumber.Text;
                item.YearOfIssue = Convert.ToInt32(YearOfIssue.Text);
                item.CarPrice = Convert.ToInt32(CarPrice.Text);
                item.RentalDayPrice = Convert.ToInt32(RentalDayPrice.Text);
                item.DateOfLastMaintenance = DateOfLastMaintenance.SelectedDate.Value.Date;
                item.EmployeeID = CbEmployee.SelectedIndex + 1;
                item.ReturnMark = (bool)ReturnMark.IsChecked;
                item.Image = Dir;

                GetContext().SaveChanges();
                Hide();
            }
            catch
            {
                MessageBox.Show("Введите корректный формат");
            }
            GetContext().SaveChanges();
            MessageBox.Show("Данные изменены");
            Hide();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var carForRemoving = GetContext().Cars.Where(p => p.ID == _current.ID).ToList();
            if (MessageBox.Show($"Вы точно хотите удалить автомобиль?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    GetContext().Cars.RemoveRange(carForRemoving);
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
