using System.Data;
using System.Data.OleDb;

namespace PlateUpWS
{
    public interface IDBContext
    {
        void OpenConnection(); //פתיחת קשר עם מסד נתונים
        void CloseConnection(); //סגירת קשר עם נתונים
        void BeginTransaction(); //פותח חבילה של פעולות ובודק אם כל הפעולות יתבצעו, או כולם או אף אחד
        void Commit(); //לאשר שכל הפעולות התבצעו
        void RollBack(); //לבטל את כל הפעולות במידה ולא כולם התבצעו
        int Delete(string sql); //מחזיר את כמות הדברים שנמחקו, לדוגמא בקשה למחוק 10 ספרים אז צריך להיות ערך מוחזר 10
        int Insert(string sql);
        int Update(string sql);
        IDataReader Select(string sql); //אובייקט שיכול לשמור את הrecordset
    }
}
