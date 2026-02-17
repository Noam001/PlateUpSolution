using Microsoft.Win32;
using Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
using WebApiClient;
using System.IO;

namespace PlateUpWpf.Frames
{
    /// <summary>
    /// Interaction logic for MealTemp.xaml
    /// </summary>
    public partial class UpdateMeal : Window
    {
        AddMealViewModel addMealView;
        string imgPath;
        public UpdateMeal()
        {
            InitializeComponent();
            GetNewMealVm();
        }

        private async Task GetNewMealVm()
        {
            WebClient<AddMealViewModel> client = new WebClient<AddMealViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/GetAddMealViewModel";
            this.addMealView = await client.GetAsync();

            if (this.addMealView != null)
            {
                this.cmbFoodTypes.ItemsSource = this.addMealView.FoodTypes;
                this.DataContext = addMealView;
                if (this.addMealView.Meal == null)
                    this.addMealView.Meal = new Meal();
            }
        }

        private void btnEditImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            bool? ok = ofd.ShowDialog();
            if (ok == true)
            {
                Uri uri = new Uri(ofd.FileName);
                this.AddImage.Source = new BitmapImage (uri); //הצגת התמונה כאשר בוחרים
                this.imgPath = ofd.FileName; // שמירת שם הקובץ

            }
        }

        private async Task btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            AddMealViewModel newMeal = new AddMealViewModel();
            newMeal.Meal.MealName = this.txtEditItemName.Text;
            newMeal.Meal.MealDescription = this.txtEditDescription.Text;
            newMeal.Meal.MealPrice = double.Parse(this.txtEditPrice.Text);
            newMeal.Meal.MealPhoto = System.IO.Path.GetExtension(this.imgPath);
            newMeal.FoodTypes = this.cmbFoodTypes.SelectedItems as List<FoodType>;
            Stream stream = new FileStream(this.imgPath, FileMode.Open, FileAccess.Read);
            WebClient<AddMealViewModel> client = new WebClient<AddMealViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/AddMeal";
            bool ok = await client.PostAsync(newMeal, stream);
            if (ok)
            {
                this.DialogResult = true;
                MessageBox.Show("Meal Added!");
                this.Close();
            }
            else
                MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
