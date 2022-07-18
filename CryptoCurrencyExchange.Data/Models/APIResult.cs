using Newtonsoft.Json;

namespace CryptoCurrencyExchange.Data.Models
{
    public partial class APIResult
    {
        [JsonProperty("data")]
        public ResponseData? Data { get; set; }
    }

    public partial class ResponseData
    {
        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, string>? Rates { get; set; }
    }
}
