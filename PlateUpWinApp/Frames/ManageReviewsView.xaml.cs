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

        private async void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Review review = (Review)btn.DataContext;
            string reviewId = review.ReviewId.ToString();

            WebClient<bool> client = new WebClient<bool>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/RemoveReview";
            client.AddParameter("reviewID", reviewId);
            bool ok = await client.GetAsync();
            if (ok)
            {
                this.reviews.Remove(review);
                this.lvReviews.ItemsSource = null;
                this.lvReviews.ItemsSource = this.reviews;
                MessageBox.Show("Review Removed!");
            }
            else
                MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
