using System.Text.Json;
using Fejka.Repository;

namespace Fejka.Test;

public class MicrosoftSerializer : ISerializer
{
    public T Clone<T>(T entity) => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(entity));
    public string Serialize<T>(T entity) => JsonSerializer.Serialize(entity);
}
