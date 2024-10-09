namespace Fejka.Repository;

public interface ISerializer
{
    T Clone<T>(T entity);
    string Serialize<T>(T entity);
}