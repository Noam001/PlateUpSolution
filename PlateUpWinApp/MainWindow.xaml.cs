using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PlateUpWinApp.Frames;

namespace PlateUpWinApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewReports viewReports;
        Login viewLogin;
        ManageOrderView orderView;
        ManageCitiesView citiesView;
        ManageFoodTypesView foodTypesView;
        ManageMenuView menuView;
        ManageReviewsView reviewsView;

        bool loginSuccess;
        public MainWindow()
        {
            InitializeComponent();
            UpdateLoginButton();
            ViewReports();
        }
        public void ViewReports()
        {
            if (this.viewReports == null)
                this.viewReports = new ViewReports();
            this.FrameContent.Content = this.viewReports;
        }
        public void ViewLogin()
        {
            if (this.viewLogin == null)
                this.viewLogin = new Login();
            this.FrameContent.Content = this.viewLogin;
        }
        public void ViewManageOrder()
        {
            if (this.orderView == null)
                this.orderView = new ManageOrderView();
            this.FrameContent.Content = this.orderView;
        }
        public void ViewManageCities()
        {
            if (this.citiesView == null)
                this.citiesView = new ManageCitiesView();
            this.FrameContent.Content = this.citiesView;
        }
        public void ViewManageFoodTypes()
        {
            if (this.foodTypesView == null)
                this.foodTypesView = new ManageFoodTypesView();
            this.FrameContent.Content = this.foodTypesView;
        }
        public void ViewManageMenu()
        {
            if (this.menuView == null)
                this.menuView = new ManageMenuView();
            this.FrameContent.Content = this.menuView;
        }
        public void ViewManageReviews()
        {
            if (this.reviewsView == null)
                this.reviewsView = new ManageReviewsView();
            this.FrameContent.Content = this.reviewsView;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)//שולח - מי שגרם לאירוע
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void UpdateLoginButton()
        {
            if (loginSuccess)
            {
                this.btnLogout.Content = "Logout";
            }
            else
            {
                this.btnLogout.Content = "Login";
            }
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
           if (!loginSuccess)
            {
                //לאסוף קלט.
                //לבדוק במסד נתונים
                //אם מסד נתונים מחזיר אמת 
            }
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            ViewReports();
        }
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            ViewManageMenu();
        }

        private void FoodTypes_Click(object sender, RoutedEventArgs e)
        {
            ViewManageFoodTypes();
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            ViewManageOrder();
        }
        private void Cities_Click(object sender, RoutedEventArgs e)
        {
            ViewManageCities();
        }

        private void Reviews_Click(object sender, RoutedEventArgs e)
        {
            ViewManageReviews();
        }
        private void FrameContent_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            ViewLogin();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


    }
}