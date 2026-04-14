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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        MainWindow mainWindow;
        public Login(MainWindow mainW)
        {
            InitializeComponent();
            mainWindow = mainW;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginModel loginModel = new LoginModel();
            loginModel.Email = this.txtBoxEmail.Text;
            loginModel.Password = this.txtBoxPass.Password;
            loginModel.IsAdmin = true;

            WebClient<LoginViewModel> client = new WebClient<LoginViewModel>();
            LoginViewModel result = client.Login(loginModel);

            if (result != null)
            {
                this.mainWindow.LoginSuccess(result);
            }
            else
                MessageBox.Show("Invalid email or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
}
