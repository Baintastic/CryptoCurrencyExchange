namespace CryptoCurrencyExchange.Data.Models
{
    public class Result : APIResult
    {
        public int StatusCode { get; set; }
        public string? Uri { get; set; }
        public string? Method { get; set; }
        public string? ResponseBody { get; set; }
    }
}
