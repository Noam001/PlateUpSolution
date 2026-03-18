using Models;
using PlateUpWpf.Frames;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebApiClient;

namespace PlateUpWinApp.Frames
{
    /// <summary>
    /// Interaction logic for ManageFoodTypesView.xaml
    /// </summary>
    public partial class ManageFoodTypesView : UserControl
    {
        List<FoodType> foodTypes;
        public ManageFoodTypesView()
        {
            InitializeComponent();
            GetFoodTypes();
        }
        private async Task GetFoodTypes()
        {
            WebClient<List<FoodType>> client = new WebClient<List<FoodType>>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/GetFoodTypes";
            this.foodTypes = await client.GetAsync();
            this.lvFoodTypes.ItemsSource = this.foodTypes;

            this.DataContext = this.foodTypes;

        }

        private async void btnAddFoodType_Click(object sender, RoutedEventArgs e)
        {
            FoodType foodtype = new FoodType();
            foodtype.FoodTypeName = this.txtNewFoodType.Text;
            WebClient<FoodType> client = new WebClient<FoodType>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/AddFoodType";
            bool ok = await client.PostAsync(foodtype);
            if (ok)
            {
                MessageBox.Show("New Food Type Added!");
                this.txtNewFoodType.Text = "";
                await GetFoodTypes(); // טוען מחדש מהשרת
            }
            else
                MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            FoodType foodType = (FoodType)btn.DataContext;
            string foodtypeId = foodType.FoodTypeId.ToString();

            WebClient<bool> client = new WebClient<bool>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/RemoveFoodType";
            client.AddParameter("foodTypeId", foodtypeId);
            bool ok = await client.GetAsync();
            if (ok)
            {
                this.foodTypes.Remove(foodType);
                this.lvFoodTypes.ItemsSource = null;
                this.lvFoodTypes.ItemsSource = this.foodTypes;
                MessageBox.Show("Food Type Removed!");
            }
            else
                MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            FoodType foodType = (FoodType)btn.DataContext;

            DataEditView editFTWindow = new DataEditView(foodType.FoodTypeName, "Food Type Name");
            bool? result = editFTWindow.ShowDialog();

            if (result == true) // אם החלון של העדכון נפתח
            {
                foodType.FoodTypeName = editFTWindow.updatedName;

                WebClient<FoodType> client = new WebClient<FoodType>();
                client.Schema = "http";
                client.Host = "localhost";
                client.Port = 5035;
                client.Path = "api/Admin/UpdateFoodType";
                bool ok = await client.PostAsync(foodType);

                if (ok) //אם העדכון הוצלח
                {
                    this.lvFoodTypes.ItemsSource = null;
                    this.lvFoodTypes.ItemsSource = this.foodTypes;
                    MessageBox.Show("Food Type Updated!");
                }
                else
                    MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
