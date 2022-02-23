using System.Text.Json.Serialization;

namespace BinaryApp.Model;

public class ResponseMessage
{
    [JsonPropertyName("echo_req")]
    public EchoRequest EchoRequest { get; set; }
    
    [JsonPropertyName("msg_type")]
    public string MessageType { get; set; }
    
    [JsonPropertyName("subscription")]
    public Subscription Subscription { get; set; }
    
    [JsonPropertyName("tick")]
    public Tick Tick { get; set; }
}

public class EchoRequest
{
    [JsonPropertyName("ticks")]
    public string Tick { get; set; }
}

public class Subscription
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class Tick
{
    [JsonPropertyName("ask")]
    public decimal Ask { get; set; }
    
    [JsonPropertyName("bid")]
    public decimal Bid { get; set; }
    
    [JsonPropertyName("epoch")]
    public object Epoch { get; set; }
    
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("pip_size")]
    public int PipSize { get; set; }
    
    [JsonPropertyName("quote")]
    public decimal Quote { get; set; }
    
    [JsonPropertyName("symbol")] 
    public string Symbol { get; set; }
}