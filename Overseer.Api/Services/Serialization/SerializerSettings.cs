using Newtonsoft.Json;

namespace Overseer.Api.Services.Serialization;

public static class SerializerSettings
{
    public static readonly JsonSerializerSettings Instance = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
    };
}
