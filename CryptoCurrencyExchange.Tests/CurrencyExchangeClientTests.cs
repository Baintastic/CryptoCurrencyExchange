
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System.Net;

namespace CryptoCurrencyExchange.Tests
{
    public class CurrencyExchangeClientTests
    {
        private IConfiguration _config;
        private string _currency;
        private string _data;
        private string _url;

        [SetUp]
        public void Setup()
        {
            _config = Substitute.For<IConfiguration>();
            _currency = "BTC";
            var apiResult = new APIResult
            {
                Data = new ResponseData
                {
                    Currency = _currency,
                    Rates = new Dictionary<string, string>()
                    {
                        { "ZAR","2000"}
                    }
                },
            };
            _data = JsonConvert.SerializeObject(apiResult);
            _url = $"http://localhost/api/currency?=";
            _config["CoinbaseAPIExchangeRateUrl"].Returns(_url);
        }
    
        [Test]
        public async Task GetLatestCurrencyExchangeRates_GivenStatusCodeOk_ShouldReturnResponseResult()
        {
            //----------------------Arrange--------------------------------
            var mockHttp = CreateMockHttpMessageHandler();

            mockHttp.When($"{_url}{_currency}")
                    .Respond("application/json", _data);

            var client = mockHttp.ToHttpClient();
            var currencyExchangeClient = CreateCurrencyExchangeClient(client);

            //----------------------Act------------------------------------
            var actual = await currencyExchangeClient.GetLatestCurrencyExchangeRates(_currency);

            //----------------------Assert---------------------------------
            Assert.That(actual?.ResponseBody, Is.EqualTo(_data));
            Assert.That(actual?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetLatestCurrencyExchangeRates_GivenBadRequest_ShouldReturnNull()
        {
            //----------------------Arrange--------------------------------
            var mockHttp = CreateMockHttpMessageHandler();

            mockHttp.When($"{_url}{_currency}")
                    .Respond(HttpStatusCode.BadRequest);

            var client = mockHttp.ToHttpClient();
            var currencyExchangeClient = CreateCurrencyExchangeClient(client);

            //----------------------Act------------------------------------
            var actual = await currencyExchangeClient.GetLatestCurrencyExchangeRates(_currency);

            //----------------------Assert---------------------------------
            Assert.IsNull(actual);
        }

        private static MockHttpMessageHandler CreateMockHttpMessageHandler()
        {
            return new MockHttpMessageHandler();
        }

        private CurrencyExchangeClient CreateCurrencyExchangeClient(HttpClient client)
        {
            return new CurrencyExchangeClient(_config, client);
        }
    }
}
