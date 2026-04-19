using Models;
using PlateUpWinApp.Frames;
using System.Reflection;
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
        Hyperlink activeLink;
        bool isAdmin;
        public MainWindow()
        {
            isAdmin = false;
            InitializeComponent();
            // ViewLogin();
            this.spNavItems.Visibility = Visibility.Visible;
            ViewReports();
        }
      
        public void LoginSuccess(LoginViewModel loginResult)
        {
            this.isAdmin = true;
            this.adminName.Text = loginResult.Name;
            this.btnLogout.Visibility = Visibility.Visible;
            this.spNavItems.Visibility = Visibility.Visible;
            SetActiveNav(this.Reports);
            ViewReports();
        }
        private void SetActiveNav(Hyperlink clicked)
        {
            // רשימת כל הניווטים
            var borders = new[] {this.BorderReports, this.MenuBorder, this.FoodTypesBorder, this.OrdersBorder, this.CitiesBorder, this.ReviewsBorder, this.LoginBorder};

            foreach (var border in borders)
            {
                // מוצאים את הלינק שבתוך ה-Border
                var textBlock = border.Child as TextBlock;
                var link = textBlock?.Inlines.FirstInline as Hyperlink;

                if (link == clicked)
                {
                    // עיצוב למצב פעיל
                    link.Foreground = Brushes.White; // צובע את הטקסט בלבן
                    border.Background = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255));// מוסיף רקע שקוף לבן 40 זה רמת השקיפות
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(251, 83, 155));
                    border.BorderThickness = new Thickness(4, 0, 0, 0);// יוצר קו רק בצד שמאל עובי 4
                }
                else
                {
                    // החזרה למצב רגיל
                    if (link != null)
                        link.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9497CD"));

                    border.Background = Brushes.Transparent;// מוריד את הרקע
                    border.BorderThickness = new Thickness(0);// מעלים את הקו הצידי
                }
            }
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
                this.viewLogin = new Login(this);
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
        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            SetActiveNav(this.Reports);
            ViewReports();
        }
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            SetActiveNav(this.Menu);
            ViewManageMenu();
        }

        private void FoodTypes_Click(object sender, RoutedEventArgs e)
        {
            SetActiveNav(this.FoodTypes);
            ViewManageFoodTypes();
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            SetActiveNav(this.Orders);
            ViewManageOrder();
        }
        private void Cities_Click(object sender, RoutedEventArgs e)
        {
            SetActiveNav(this.Cities);
            ViewManageCities();
        }

        private void Reviews_Click(object sender, RoutedEventArgs e)
        {
            SetActiveNav(this.Reviews);
            ViewManageReviews();
        }
        private void FrameContent_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            SetActiveNav(this.Login);
            ViewLogin();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.adminName.Text = "";
            this.spNavItems.Visibility = Visibility.Collapsed;
            ViewLogin();
        }
    }
}