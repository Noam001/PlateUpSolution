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

namespace PlateUpWinApp.Frames
{
    /// <summary>
    /// Interaction logic for ManageMenuView.xaml
    /// </summary>
    public partial class ManageMenuView : UserControl
    {
        public ManageMenuView()
        {
            InitializeComponent();
        }

        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            // פתיחת חלון לבחירת תמונה
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                // כאן תוכלי לשמור את הנתיב או להציג את התמונה
                MessageBox.Show($"Selected image: {filePath}");
            }
        }

        private void btnEditImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                MessageBox.Show($"Selected image: {filePath}");
            }
        }
    }
}
