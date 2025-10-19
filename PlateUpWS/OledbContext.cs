using System.Data;
using System.Data.OleDb;

namespace PlateUpWS
{
    public class OledbContext : IDBContext
    {
        OleDbConnection connection; //אובייקט שפותח קשר עם מסד נתונים וסוגר- אחראי על הקשר עם מסד נתונים
        OleDbCommand command; //אובייקט שאחראי להעביר את הפקודות למסד נתונים
        OleDbTransaction transaction; //

        public OledbContext()
        {
            this.connection = new OleDbConnection();
            this.connection.ConnectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Directory.GetCurrentDirectory()}\App_Data\PlateUpDataBase.accdb"; //מה סוג של מסד נתונים- ACSESS, ומיקום הקובץ
            this.command = new OleDbCommand();
            this.command.Connection = this.connection;
        }
        public void BeginTransaction()
        {
            this.transaction = this.connection.BeginTransaction();
        }

        public void CloseConnection()
        {
            this.connection.Close();
        }

        public void Commit()
        {
            this.transaction.Commit();
        }

        public int Delete(string sql)
        {
            return ChangeDb(sql);
        }

        public int Insert(string sql)
        {
            return ChangeDb(sql);
        }

        public void OpenConnection()
        { 
            this.connection.Open();
        }

        public void RollBack()
        {
            this.transaction.Rollback();
        }

        public IDataReader Select(string sql)
        {
            this.command.CommandText = sql;
            return this.command.ExecuteReader();
        }

        public int Update(string sql)
        {
            return ChangeDb(sql);
        }
        private int ChangeDb(string sql)
        {
            this.command.CommandText = sql;
            return this.command.ExecuteNonQuery();
        }
    }
}
