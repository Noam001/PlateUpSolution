using Microsoft.Win32;
using Models;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebApiClient;

namespace PlateUpWpf.Frames
{
    /// <summary>
    /// Interaction logic for MealTemp.xaml
    /// </summary>
    public partial class ManageMeal : Window
    {
        MealViewModel addMealView;
        Meal meal;
        FoodType foodType;
        string imgPath;
        public ManageMeal()
        {
            InitializeComponent();
            this.meal = null;
            this.foodType = null;
            GetNewMealVm();
            this.sumbitBtn.Click += (s, e) => AddNewMealBtn();
        }
        public ManageMeal(Meal meal, FoodType foodType)
        {
            InitializeComponent();
            this.meal = new Meal { 
                 MealDescription=meal.MealDescription,
                 MealId=meal.MealId,
                 MealName=meal.MealName,
                 MealPhoto = meal.MealPhoto,
                 MealPrice = meal.MealPrice,
                 MealStatus = meal.MealStatus 
            };
            this.foodType = foodType;
            this.ImageClickDesign.Visibility = Visibility.Collapsed;
            this.sumbitBtn.Content = "Update";
            this.sumbitBtn.Click += (s, e) => UpdateMeal(meal.MealId);
            GetNewMealVm();
        }
        private async void UpdateMeal(int mealId)
        {
            MealViewModel updateMeal = new MealViewModel();
            updateMeal.Meal = new Meal();
            updateMeal.Meal.MealId = mealId;
            updateMeal.Meal.MealName = this.txtEditItemName.Text;
            updateMeal.Meal.MealDescription = this.txtEditDescription.Text;
            updateMeal.Meal.MealPrice = double.Parse(this.txtEditPrice.Text);
            updateMeal.Meal.MealStatus = this.cmbEditStatus.SelectedValue == "True";

            FoodType selectedFoodType = this.cmbFoodTypes.SelectedItem as FoodType;
            updateMeal.FoodTypes = new List<FoodType>() { selectedFoodType };
            WebClient<MealViewModel> client = new WebClient<MealViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/UpdateMeal";
            updateMeal.Meal.Validate();
            bool isValid = updateMeal.Meal.IsValid;
            if (this.imgPath != null && isValid==true)
            {
                updateMeal.Meal.MealPhoto = System.IO.Path.GetExtension(this.imgPath);
                Stream stream = new FileStream(this.imgPath, FileMode.Open, FileAccess.Read);
                bool ok = await client.PostAsync(updateMeal, stream);
                HandleResult(ok);
            }
            else
            {
                Stream stream = Stream.Null;
                bool ok = await client.PostAsync(updateMeal, stream);
                HandleResult(ok);
            }
        }
        private void HandleResult(bool ok)
        {
            if (ok)
            {
                this.DialogResult = true;
                MessageBox.Show("Meal Updated!");

                this.Close();
            }
            else
                MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private async Task GetNewMealVm()
        {
            WebClient<MealViewModel> client = new WebClient<MealViewModel>();

            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/GetAddMealViewModel";
            this.addMealView = await client.GetAsync();

            if (this.addMealView != null)
            {

                this.cmbFoodTypes.ItemsSource = this.addMealView.FoodTypes;              
                if (this.meal == null)
                    this.addMealView.Meal = new Meal();
                else
                {
                    this.addMealView.Meal = this.meal;
                    this.cmbFoodTypes.SelectedValue = this.foodType.FoodTypeId;
                    string imageUrl = $"http://localhost:5035/DataImages/{meal.MealPhoto}";
                    this.AddImage.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                    this.cmbEditStatus.SelectedIndex = this.addMealView.Meal.MealStatus ? 0 : 1;
                }

                this.DataContext = addMealView;
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
                this.imgPath = ofd.FileName; // שמירת נתיב הקובץ
                this.btnEditImage.Visibility = Visibility.Collapsed;// מסתיר את הכפתור עם הטקסט

            }
        }

        private async void AddNewMealBtn()
        {
            txtEditItemName.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            txtEditDescription.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            MealViewModel newMeal = new MealViewModel();
            newMeal.Meal = new Meal();
            newMeal.Meal.MealName = this.txtEditItemName.Text;
            newMeal.Meal.MealDescription = this.txtEditDescription.Text;

            double n;
            bool isNumeric = double.TryParse(this.txtEditPrice.Text, out n);
            newMeal.Meal.MealPrice = isNumeric ? n : -1;

            newMeal.Meal.MealPhoto = string.IsNullOrEmpty(imgPath) ? null : System.IO.Path.GetExtension(this.imgPath); //שומר את סוג הפורמט
            FoodType selectedFoodType = this.cmbFoodTypes.SelectedItem as FoodType;
            newMeal.FoodTypes = new List<FoodType>() { selectedFoodType };
            newMeal.Meal.MealStatus = this.cmbEditStatus.SelectedValue == "True";

            this.MealPhotoEdit.Text = newMeal.Meal.MealPhoto;
           // this.FoodTypeSelect.Text = selectedFoodType == null ? null : selectedFoodType.FoodTypeName;

            Stream stream = null;
            if (newMeal.Meal.MealPhoto != null)
                stream = new FileStream(this.imgPath, FileMode.Open, FileAccess.Read);

            newMeal.Meal.Validate();
            bool isMealValid = newMeal.Meal.IsValid;

            if (isMealValid)
            {
                WebClient<MealViewModel> client = new WebClient<MealViewModel>();
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
}
