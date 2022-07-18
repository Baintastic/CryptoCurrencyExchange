using CryptoCurrencyExchange.Core.IServices;
using CryptoCurrencyExchange.Core.Model;
using CryptoCurrencyExchange.Data.ExternalServices;
using CryptoCurrencyExchange.Data.IRepositories;
using CryptoCurrencyExchange.Data.Models;
using CryptoCurrencyExchange.Data.Models.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace CryptoCurrencyExchange.Core.Services
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly ICurrencyExchangeClient _currencyExchangeClient;
        private readonly IResponseRepository _responseRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration Configuration;

        public CurrencyExchangeService(ICurrencyExchangeClient currencyExchangeClient,
            IResponseRepository responseRepository,
            IRequestRepository requestRepository,
            IMemoryCache memoryCache,
            IConfiguration configuration)
        {
            _currencyExchangeClient = currencyExchangeClient;
            _responseRepository = responseRepository;
            _requestRepository = requestRepository;
            _memoryCache = memoryCache;
            Configuration = configuration;
        }

        public async Task<CurrencyExchange> GetCurrencyExchangeRates(string currency)
        {
            if (!IsCurrencyExchangeRateDataCached(out CurrencyExchange cachedCurrencyExchangeRates))
            {
                cachedCurrencyExchangeRates = await GetLatestCurrencyExchangeRates(currency, cachedCurrencyExchangeRates);
            }
            return cachedCurrencyExchangeRates;
        }

        private async Task<CurrencyExchange> GetLatestCurrencyExchangeRates(string currency, CurrencyExchange cachedCurrencyExchange)
        {
            var latestCurrencyExchangeRatesResult =  _currencyExchangeClient.GetLatestCurrencyExchangeRates(currency).Result;
            if (latestCurrencyExchangeRatesResult != null)
            {
                cachedCurrencyExchange = new CurrencyExchange
                {
                    BaseCurrency = latestCurrencyExchangeRatesResult?.Data?.Currency,
                    Rates = latestCurrencyExchangeRatesResult?.Data?.Rates
                };
                var cacheDuration = TimeSpan.FromMinutes(Convert.ToDouble(Configuration["CacheDuration"]));
                _memoryCache.Set("exchangeRate", cachedCurrencyExchange, cacheDuration);

                var requestId = await SaveUserRequest(latestCurrencyExchangeRatesResult);
                await SaveResponse(latestCurrencyExchangeRatesResult, requestId);
            }
            return cachedCurrencyExchange;
        }

        private bool IsCurrencyExchangeRateDataCached(out CurrencyExchange cachedCurrencyExchange)
        {
            return _memoryCache.TryGetValue("exchangeRate", out cachedCurrencyExchange);
        }

        private async Task SaveResponse(Result? result, int requestId)
        {
            var response = new Response
            {
                Body = result?.ResponseBody,
                StatusCode = result?.StatusCode,
                UserRequestId = requestId,
                ResponseDate = DateTime.Now,
            };
            await _responseRepository.AddResponse(response);
        }

        private async Task<int> SaveUserRequest(Result? result)
        {
            var request = new UserRequest
            {
                Url = result?.Uri,
                RequestMethod = result?.Method,
                RequestDate = DateTime.Now,
            };
            await _requestRepository.AddRequest(request);
            return request.Id;
        }
    }
}
