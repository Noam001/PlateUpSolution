using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    // מודל בקשה להוספת מנה להזמנה.
    // משמש להעברת נתוני הבקשה מהלקוח לשרת בעת הוספת מנה לעגלת הקניות.
    public class AddMealRequest
    {
        public int MealId { get; set; } // מזהה ייחודי של המנה המבוקשת
        public int Quantity { get; set; } // כמות המנות המבוקשת
        public string? MealNotes { get; set; } // הערה אישית של הלקוח למנה (אופציונלי)
        public string ClientId { get; set; } // מזהה הלקוח המבצע את הבקשה, נלקח מה-Session
    }
    // מודל פריט עגלה המייצג מנה בתוך הזמנה פעילה.
    /// יורש מ-AddMealRequest ומרחיב אותו בפרטי התצוגה הנדרשים לממשק המשתמש.
    public class CartItem : AddMealRequest
    {
        public int OrderID { get; set; } // מזהה ההזמנה (העגלה) שאליה שייכת המנה
        public string MealName { get; set; } // שם המנה לתצוגה בעגלה
        public string MealPhoto { get; set; } // שם קובץ התמונה של המנה (כולל סיומת) לטעינה מתיקיית השרת
        public double MealPrice { get; set; }// מחיר המנה בעת ההזמנה
    }
}
