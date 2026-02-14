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
    }
}
