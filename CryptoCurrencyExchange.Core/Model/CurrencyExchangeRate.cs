namespace CryptoCurrencyExchange.Core.Model
{
    public class CurrencyExchange
    {
        public string? BaseCurrency { get; set; }
        public Dictionary<string, string>? Rates { get; set; }
    }
}
