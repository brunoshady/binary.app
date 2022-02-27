using System.Text.Json;
using BinaryApp.Model;

namespace BinaryApp.Utils;

public static class JsonHandler
{
    public static object Deserialize(string json)
    {
        var authorizeResponse = JsonSerializer.Deserialize<AuthorizeResponse>(json);

        if (authorizeResponse is {Authorize: { }})
            return authorizeResponse.Authorize;

        var tickResponse = JsonSerializer.Deserialize<TickResponse>(json);

        if (tickResponse is {Tick: { }})
            return tickResponse.Tick;
        
        return null;
    }

    public static string Serialize(object @object)
    {
        return JsonSerializer.Serialize(@object);
    }
}