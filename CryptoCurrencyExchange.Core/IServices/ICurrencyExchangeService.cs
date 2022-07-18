using CryptoCurrencyExchange.Core.Model;

namespace CryptoCurrencyExchange.Core.IServices
{
    public interface ICurrencyExchangeService
    {
        Task<CurrencyExchange> GetCurrencyExchangeRates(string currency);
    }
}
