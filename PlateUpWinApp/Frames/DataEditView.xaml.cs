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
    /// Interaction logic for EditFoodType.xaml
    /// </summary>
    public partial class DataEditView : Window
    {
        public string updatedName;
        public DataEditView(string currentName, string label) //currentname - Default name, label - is it foodtype or city.
        {
            InitializeComponent();
            this.Title = $"Edit {label}";
            this.txtLabel.Text = label;        // "Food Type Name:" או "City Name:"
            this.txtName.Text = currentName;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.updatedName = this.txtName.Text;
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
