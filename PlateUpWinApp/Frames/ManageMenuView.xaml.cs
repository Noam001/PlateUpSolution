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
using Models;
using PlateUpWpf.Frames;
using WebApiClient;

namespace PlateUpWinApp.Frames
{
    /// <summary>
    /// Interaction logic for ManageMenuView.xaml
    /// </summary>
    public partial class ManageMenuView : UserControl
    {
        ManageMenuViewModel menuViewModel;

        public ManageMenuView()
        {
            InitializeComponent();
            GetManageMenuViewModel();
        }
        private async Task GetManageMenuViewModel(string foodTypeId = "-1")
        {
            WebClient<ManageMenuViewModel> client = new WebClient<ManageMenuViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/GetManageMenuViewModel";
            client.AddParameter("foodTypeId", foodTypeId);
            this.menuViewModel = await client.GetAsync();
            this.lvMeals.ItemsSource = this.menuViewModel.Meals;
            if (this.cmbFoodTypes.ItemsSource == null)
            {
                this.menuViewModel.FoodTypes.Insert(0, new FoodType { FoodTypeId = -1, FoodTypeName = "All" });
                this.cmbFoodTypes.ItemsSource = this.menuViewModel.FoodTypes;
            }
            this.DataContext = this.menuViewModel;

        }

        private async void AddMealButton_Click(object sender, RoutedEventArgs e)
        {
            ManageMeal meal = new ManageMeal();
            bool? result = meal.ShowDialog();
            if (result == true)
                await GetManageMenuViewModel();
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Meal meal = (Meal)btn.DataContext;
            WebClient<bool> client = new WebClient<bool>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/RemoveMeal";
            client.AddParameter("mealPhoto", meal.MealPhoto);
            client.AddParameter("mealId", meal.MealId.ToString());
            bool ok = await client.GetAsync();

            if (ok)
            {
                this.menuViewModel.Meals.Remove(meal);
                this.lvMeals.ItemsSource = null;
                this.lvMeals.ItemsSource = this.menuViewModel.Meals;
                MessageBox.Show("Meal Removed!");
            }
            else
                MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void cmbFoodTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.lvMeals == null) return; //בדיקה האם רכיב התצוגה כבר נוצר בזיכרון, כדי למנוע קריסה של התוכנה אם האירוע מופעל מוקדם מדי.
            if (this.cmbFoodTypes.SelectedValue == null) return;
            string foodTypeId = this.cmbFoodTypes.SelectedValue.ToString();
            await GetManageMenuViewModel(foodTypeId);

        }

        private async void ViewUpdateBtn(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Meal meal = (Meal)btn.DataContext;
            WebClient<FoodType> client = new WebClient<FoodType>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/GetFoodTypeByMealId";
            client.AddParameter("mealId", meal.MealId.ToString());
            FoodType foodType = await client.GetAsync();
            ManageMeal manageMeal = new ManageMeal(meal, foodType);
            bool? result = manageMeal.ShowDialog();
            if (result == true)
                await GetManageMenuViewModel();
        }
    }
}
