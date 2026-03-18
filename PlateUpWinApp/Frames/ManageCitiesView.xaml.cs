using Models;
using PlateUpWpf.Frames;
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
using WebApiClient;

namespace PlateUpWinApp.Frames
{
    /// <summary>
    /// Interaction logic for ManageCitiesView.xaml
    /// </summary>
    public partial class ManageCitiesView : UserControl
    {
        List<City> cities;
        public ManageCitiesView()
        {
            InitializeComponent();
            GetCities();
        }
        private async Task GetCities()
        {
            WebClient<List<City>> client = new WebClient<List<City>>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/GetCities";
            this.cities = await client.GetAsync();
            this.lvCities.ItemsSource = this.cities;

            this.DataContext = this.cities;

        }

        private async void btnAddCity_Click(object sender, RoutedEventArgs e)
        {
            City city = new City();
            city.CityName = this.txtNewCity.Text;
            WebClient<City> client = new WebClient<City>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/AddCity";
            bool ok = await client.PostAsync(city);
            if (ok)
            {
                MessageBox.Show("New City Added!");
                this.txtNewCity.Text = "";
                await GetCities(); // טוען מחדש מהשרת
            }
            else
                MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            City city = (City)btn.DataContext;

            DataEditView editCityWindow = new DataEditView(city.CityName, "City Name");
            bool? result = editCityWindow.ShowDialog();

            if (result == true) // אם החלון של העדכון נפתח
            {
                city.CityName = editCityWindow.updatedName; //עדכון שם העיר

                WebClient<City> client = new WebClient<City>();
                client.Schema = "http";
                client.Host = "localhost";
                client.Port = 5035;
                client.Path = "api/Admin/UpdateCity";
                bool ok = await client.PostAsync(city);

                if (ok) //אם העדכון הוצלח
                {
                    this.lvCities.ItemsSource = null;
                    this.lvCities.ItemsSource = this.cities; // עדכון רשימת הערים
                    MessageBox.Show("City Updated!");
                }
                else
                    MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            City city = (City)btn.DataContext;
            string cityId = city.CityId.ToString();

            WebClient<bool> client = new WebClient<bool>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/RemoveCity";
            client.AddParameter("cityId", cityId);
            bool ok = await client.GetAsync();
            if (ok)
            {
                this.cities.Remove(city);
                this.lvCities.ItemsSource = null;
                this.lvCities.ItemsSource = this.cities;
                MessageBox.Show("City Removed!");
            }
            else
                MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
