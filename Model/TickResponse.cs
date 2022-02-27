using System;
using System.Text.Json.Serialization;

namespace BinaryApp.Model;

public class TickResponse
{
    [JsonPropertyName("tick")]
    public Tick Tick { get; set; }
}

public class Tick
{
    private object _epoch;

    [JsonPropertyName("ask")]
    public decimal Ask { get; set; }
    
    [JsonPropertyName("bid")]
    public decimal Bid { get; set; }

    [JsonPropertyName("epoch")]
    public object Epoch
    {
        get => _epoch;
        set
        {
            _epoch = value;

            if (!long.TryParse(value.ToString(), out var epochAsLong))
                return;
            
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(epochAsLong);
            EpochDateTime = dateTimeOffset.DateTime;
        }
    }

    public DateTime EpochDateTime { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("pip_size")]
    public int PipSize { get; set; }
    
    [JsonPropertyName("quote")]
    public decimal Quote { get; set; }
    
    [JsonPropertyName("symbol")] 
    public string Symbol { get; set; }
}