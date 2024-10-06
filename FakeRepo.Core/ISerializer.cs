namespace FakeRepo.Core;

public interface ISerializer
{
    T Clone<T>(T entity);
    string Serialize<T>(T entity);
}