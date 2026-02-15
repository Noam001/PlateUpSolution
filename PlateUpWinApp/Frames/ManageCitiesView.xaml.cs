using Models;
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
    }
}
