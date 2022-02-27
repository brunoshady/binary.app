using System.Text.Json.Serialization;

namespace BinaryApp.Model;

public class AuthorizeResponse
{
    [JsonPropertyName("authorize")]
    public Authorize Authorize { get; set; }
}

public class Authorize
{
    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }
    
    [JsonPropertyName("country")]
    public string Country { get; set; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}