using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PlateUpWpf.Frames
{
    public class ImageUrlToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // בניית כתובת URL מלאה לתמונה בשרת לפי שם הקובץ
            string url = $"http://localhost:5035/DataImages/{value.ToString()}";
            if (value == null)
                return null;
            try
            {
                // יצירת BitmapImage מהכתובת המלאה לטעינת התמונה מהשרת
                return new BitmapImage(new Uri(url, UriKind.Absolute));
            }
            catch
            {
                // במקרה של כשל בטעינה (שרת לא זמין, קובץ לא קיים וכו
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
