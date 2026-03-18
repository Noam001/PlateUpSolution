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
using System.Windows.Shapes;

namespace PlateUpWpf.Frames
{
    /// <summary>
    /// Interaction logic for ViewOrderWindow.xaml
    /// </summary>
    public partial class ViewOrderWindow : Window
    {
        public ViewOrderWindow(Order order, CartViewModel orderedMeals)
        {
            InitializeComponent();
            txtOrderId.Text = order.OrderId.ToString();
            txtClientId.Text = order.ClientId;
            txtOrderDate.Text = order.OrderDate;
            txtOrderTime.Text = order.OrderTime;
            txtNumOfPeople.Text = order.NumOfPeople.ToString();
            txtStatus.Text = order.OrderStatus ? "Completed" : "Pending";
            this.lvMeals.ItemsSource = orderedMeals.CartItems;
            if(orderedMeals.CartItems.Count == 0) //אם לא קיים מנות להזמנה
            {
                this.txtTableReservation.Visibility = Visibility.Visible;
            }
            this.DataContext = orderedMeals;
        }
    }
}
