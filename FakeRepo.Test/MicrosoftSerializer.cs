using System.Text.Json;
using FakeRepo.Core;

namespace FakeRepo.Test;

public class MicrosoftSerializer : ISerializer
{
    public T Clone<T>(T entity)
    {
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(entity));
    }

    public string Serialize<T>(T entity)
    {
        return JsonSerializer.Serialize(entity);
    }
}
