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
    /// Interaction logic for ViewReports.xaml
    /// </summary>
    public partial class ViewReports : UserControl
    {
        ReportsViewModel reportsViewModel;
        DateTime? dateFrom;
        DateTime? dateTo;  
        public ViewReports()
        {
            InitializeComponent();
            GetReports();
        }
        private async Task GetReports(string foodTypeId = "-1")
        {
            WebClient<ReportsViewModel> client = new WebClient<ReportsViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/GetManageMenuViewModel";
            client.AddParameter("foodTypeId", foodTypeId);
            this.reportsViewModel = await client.GetAsync();
            
            this.DataContext = this.reportsViewModel;

        }

        private void datePickerRangefrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.dateFrom = this.datePickerRangefrom.SelectedDate;
        }

        private void datePickerRangeto_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.dateTo = this.datePickerRangefrom.SelectedDate;
            if(dateFrom <= this.dateTo)
            {
                string theDatefrom = dateFrom.ToString().Substring(0,10);
                string theDateto = dateFrom.ToString().Substring(0,10);

                MessageBox.Show($"All right {theDatefrom}>{theDateto}");
            }
            
        } 
    }
}
