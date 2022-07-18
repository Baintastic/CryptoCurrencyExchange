using CryptoCurrencyExchange.Core.IServices;
using CryptoCurrencyExchange.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CryptoCurrencyExchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Gets the latest Crypto Currency exchange rates")]
    public class CurrencyExchangeController : ControllerBase
    {
        private readonly ILogger<CurrencyExchangeController> _logger;
        private readonly ICurrencyExchangeService _currencyExchangeService;

        public CurrencyExchangeController(ILogger<CurrencyExchangeController> logger, ICurrencyExchangeService currencyExchangeService)
        {
            _logger = logger;
            _currencyExchangeService = currencyExchangeService;
        }

        [HttpGet("rates/{currency}")]
        public async Task<CurrencyExchange> GetLatestCurrencyExchangeRates(string currency)
        {
            return await _currencyExchangeService.GetCurrencyExchangeRates(currency);
        }
    }
}
