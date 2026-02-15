using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PlateUpWpf.Frames
{
    public class StarColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // בדיקה למקרה שהערך ריק
            if (value == null || parameter == null)
                return Brushes.Gray;

            int rating; //הדירוג שהלקוח נתן 1–5
            int starIndex; //מספר הכוכב שנבדק כרגע

            if (!int.TryParse(value.ToString(), out rating))
                return Brushes.Gray;

            if (!int.TryParse(parameter.ToString(), out starIndex))
                return Brushes.Gray;

            // אם מספר הכוכב קטן או שווה לדירוג → זהב
            if (starIndex <= rating)
                return Brushes.Gold;

            // אחרת אפור
            return Brushes.Gray;
            //int rating = (int)value; גרסה פשוטה בהרבה אך פחות בטוחה
            //int starIndex = int.Parse(parameter.ToString());

            //if (starIndex <= rating)
            //    return Brushes.Gold;

            //return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
