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

        private void Menu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FoodTypes_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}