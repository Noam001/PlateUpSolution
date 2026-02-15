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
    /// Interaction logic for ManageReviewsView.xaml
    /// </summary>
    public partial class ManageReviewsView : UserControl
    {
        List<Review> reviews;
        public ManageReviewsView()
        {
            InitializeComponent();
            GetReviews();
        }
        private async Task GetReviews()
        {
            WebClient<List<Review>> client = new WebClient<List<Review>>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/GetReviews";
            this.reviews = await client.GetAsync();
            this.lvReviews.ItemsSource = this.reviews;
        }
    }
}
