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
        // המרת ציון הדירוג המספרי של הלקוח לצבע הכוכב המתאים במסך
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // מניעת קריסה ובדיקה למקרה שהערך או מספר הכוכב ריקים
            if (value == null || parameter == null)
                return Brushes.Gray;

            int rating; //הדירוג שהלקוח נתן 1–5
            int starIndex; // מספר הכוכב הספציפי שנבדק כרגע במסך

            // המרת הנתונים לערכים מספריים חוקיים, אם נכשל יוחזר צבע אפור
            if (!int.TryParse(value.ToString(), out rating))
                return Brushes.Gray;
            if (!int.TryParse(parameter.ToString(), out starIndex))
                return Brushes.Gray;

            // אם מספר הכוכב הנוכחי קטן או שווה לציון הדירוג הכולל -> צביעה בזהב
            if (starIndex <= rating)
                return Brushes.Gold;

            // אם מספר הכוכב גבוה מהציון שניתן -> צביעה באפור
            return Brushes.Gray;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
