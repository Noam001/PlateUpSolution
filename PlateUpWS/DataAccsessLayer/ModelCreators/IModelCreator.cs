using System.Data;
namespace PlateUpWS
{
    public interface IModelCreator<T>
    {
        T CreateModel(IDataReader reader); //ליצור מודל מהמקור (מידע) של הRecordSet
        
    }
}
