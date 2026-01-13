namespace PlateUpWS
{
    public interface IRepository<T>
    {
        List<T> GetAll(); //רשימה של המודל

        T GetById(string id);
        bool Create(T item);
        bool Update(T item);
        bool Delete(string id);
    }
}
