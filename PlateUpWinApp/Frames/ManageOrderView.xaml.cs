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
    /// Interaction logic for ManageOrderView.xaml
    /// </summary>
    public partial class ManageOrderView : UserControl
    {
        ManageOrdersViewModel manageOrdersView;
        public ManageOrderView()
        {
            InitializeComponent();
            GetManageOrdersViewModel();
        }
        private async Task GetManageOrdersViewModel(int orderID = 0, bool? orderStatus = null)
        {
            WebClient<ManageOrdersViewModel> client = new WebClient<ManageOrdersViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/GetManageOrdersViewModel";
            if(orderID != 0)
                client.AddParameter("orderID", orderID.ToString());
            if (orderStatus != null)
                client.AddParameter("orderStatus", orderStatus.ToString());
            this.manageOrdersView = await client.GetAsync();
            this.lvOrders.ItemsSource = this.manageOrdersView.Orders;
            this.DataContext = this.manageOrdersView;

        }

        private async void txtSearchOrder_TextChanged(object sender, TextChangedEventArgs e) //חיפוש לפי ID
        {
            if (int.TryParse(txtSearchOrder.Text, out int orderId)) //מנסה לתרגם טקסט למספר ומצליח רק אם הטקסט הוא באמת מספר
                await GetManageOrdersViewModel(orderID: orderId);
            else
                await GetManageOrdersViewModel();
        }

        private async void cmbOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e) //מיון לפי סטטוס ההזמנה
        {
            if (this.lvOrders == null) return; //בדיקה האם רכיב התצוגה כבר נוצר בזיכרון, כדי למנוע קריסה של התוכנה אם האירוע מופעל מוקדם מדי.

            ComboBoxItem selected = (ComboBoxItem)this.cmbOrderStatus.SelectedItem;

            if (selected.Content.ToString() != "All")
            {
                bool status = selected.Content.ToString() == "Completed";
                await GetManageOrdersViewModel(orderStatus: status);
            }
            else
            {
                await GetManageOrdersViewModel();
            }
        }

        private async void btnUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Order order = (Order)btn.DataContext;

            // StackPanel הראשי 
            StackPanel mainPanel = (StackPanel)((StackPanel)btn.Parent).Parent;
            ComboBox cmb = (ComboBox)mainPanel.Children[1]; // cmbStatus1 הוא השני בתוך הStackPanel
            bool newStatus = (bool)cmb.SelectedValue;

            WebClient<bool> client = new WebClient<bool>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/UpdateOrderStatus";
            client.AddParameter("orderId", order.OrderId.ToString());
            client.AddParameter("orderStatus", newStatus.ToString());
            bool ok = await client.GetAsync();

            if (ok)
                MessageBox.Show("Order Status updated!");
            else
                MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Order order = (Order)btn.DataContext;
            WebClient<bool> client = new WebClient<bool>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/RemoveOrder";
            client.AddParameter("orderID", order.OrderId.ToString());
            bool ok = await client.GetAsync();

            if (ok)
            {
                this.manageOrdersView.Orders.Remove(order);
                this.lvOrders.ItemsSource = null;
                this.lvOrders.ItemsSource = this.manageOrdersView.Orders;
                MessageBox.Show("Order Removed!");
            }             
            else
                MessageBox.Show("Failed, try again later.", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void btnViewDetails_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Order order = (Order)btn.DataContext;
            WebClient<CartViewModel> client = new WebClient<CartViewModel>();
            client.Schema = "http";
            client.Host = "localhost";
            client.Port = 5035;
            client.Path = "api/Admin/ViewOrderedMeals";
            client.AddParameter("orderID", order.OrderId.ToString());
            CartViewModel orderedMeals = await client.GetAsync();
            ViewOrderWindow viewOrder = new ViewOrderWindow(order, orderedMeals);
            viewOrder.ShowDialog();
        }
    }
}
