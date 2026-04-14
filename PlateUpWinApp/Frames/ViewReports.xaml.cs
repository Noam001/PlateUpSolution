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
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PlateUpWinApp.Frames
{
    /// <summary>
    /// Interaction logic for ViewReports.xaml
    /// </summary>
    public partial class ViewReports : UserControl
    {
        ReportsViewModel reportsViewModel;
        OrderReport orderReport;
        DateTime? dateFrom;
        DateTime? dateTo;  
        public ViewReports()
        {
            InitializeComponent();
            GetReports();
        }
        private async Task GetReports(string? fromDate = null, string? toDate = null)
        {
            WebClient<ReportsViewModel> client = new WebClient<ReportsViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/GetReports";
            client.AddParameter("fromDate", fromDate);
            client.AddParameter("toDate", toDate);
            this.reportsViewModel = await client.GetAsync();

            this.DataContext = this.reportsViewModel;

        }

        private void datePickerRangefrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.dateFrom = this.datePickerRangefrom.SelectedDate;
        }

        private async void datePickerRangeto_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.dateTo = this.datePickerRangeto.SelectedDate;
            if (this.datePickerRangeto.SelectedDate == null)
                return;

            // בדיקה שנבחרו שני תאריכים
            if (this.dateFrom == null)
            {
                MessageBox.Show("Please select a From date as well.", "Missing Date");
                this.datePickerRangeto.SelectedDate = null;
            }

            // בדיקה ש-FROM לפני TO
            if (this.dateFrom > this.dateTo)
            {
                MessageBox.Show("The 'From' date must be before the 'To' date.", "Invalid Date Range");
                this.datePickerRangeto.SelectedDate = null;
                this.dateTo = null;
            }

            WebClient<OrderReport> client = new WebClient<OrderReport>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/GetOrderReport";
            client.AddParameter("fromDate", this.dateTo.ToString());
            client.AddParameter("toDate", this.dateFrom.ToString());
            this.orderReport = await client.GetAsync();

            this.reportsViewModel.TotalOrdersInDateRange = this.orderReport.TotalOrdersInDateRange;
            this.reportsViewModel.TotalIncomeInDateRange = this.orderReport.TotalIncomeInDateRange;
            this.DataContext = null;
            this.DataContext = this.reportsViewModel;
        }
    }
}
