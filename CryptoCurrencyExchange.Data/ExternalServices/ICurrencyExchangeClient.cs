using CryptoCurrencyExchange.Data.Models;

namespace CryptoCurrencyExchange.Data.ExternalServices
{
    public interface ICurrencyExchangeClient
    {
        Task<Result?> GetLatestCurrencyExchangeRates(string currency);
    }
}
