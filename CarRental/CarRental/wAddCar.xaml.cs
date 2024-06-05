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
    /// Логика взаимодействия для wAddCar.xaml
    /// </summary>
    public partial class wAddCar : Window
    {
        private static Employees _currentEmployee = new Employees();
        private static List<Employees> _listEmployees = new List<Employees>();
        private string DirAuto = @"Resources\img\emptycar.png";
        private string Dir1 = "";
        private string Dir = "";

        public wAddCar()
        {
            InitializeComponent();
            UpLoad();
        }

        private void UpLoad()
        {
            _listEmployees = GetContext().Employees.ToList();
            CbCarBrand.ItemsSource = GetContext().CarBrands.ToList();
            CbCarBrand.SelectedIndex = 0;
            CbEmployee.ItemsSource = _listEmployees.Where(x => x.Positions.Position_name == "Механик").ToList();
            CbEmployee.SelectedIndex = 0;
            YearOfIssue.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            CarPrice.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            RentalDayPrice.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
        }

        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[0-9.]").Success) e.Handled = true;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Dir1 == "") Dir = DirAuto;
            else Dir = Dir1;

            try
            {
                _currentEmployee = _listEmployees.Where(x => x.FIO == CbEmployee.Text).First();
                var item = new Cars
                {
                    ID = GetContext().Cars.ToList().Max(point => point.ID) + 1,
                    BrandID = CbCarBrand.SelectedIndex + 1,
                    RegistrationNumber = RegistrationNumber.Text,
                    BodyNumber = BodyNumber.Text,
                    EngineNumber = EngineNumber.Text,
                    YearOfIssue = Convert.ToInt32(YearOfIssue.Text),
                    CarPrice = Convert.ToInt32(CarPrice.Text),
                    RentalDayPrice = Convert.ToInt32(RentalDayPrice.Text),
                    DateOfLastMaintenance = DateOfLastMaintenance.SelectedDate.Value.Date,
                    EmployeeID = _currentEmployee.ID,
                    ReturnMark = (bool)ReturnMark.IsChecked,
                    Image = Dir
                };

                GetContext().Cars.Add(item);
                GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog { };
            try
            {
                if (file.ShowDialog() == true)
                {
                    string filename = file.FileName;
                    string fileN = System.IO.Path.GetFileNameWithoutExtension(filename);
                    string ext = System.IO.Path.GetExtension(filename);
                    Dir1 = filename;
                    addPhotoButton.Content = Dir1;
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
    }
}
