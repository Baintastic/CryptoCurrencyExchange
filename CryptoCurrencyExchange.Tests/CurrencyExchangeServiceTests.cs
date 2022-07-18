
using CryptoCurrencyExchange.Core.Model;

namespace CryptoCurrencyExchange.Tests
{
    [TestFixture]
    public class CurrencyExchangeServiceTests
    {
        private ICurrencyExchangeClient _currencyExchangeClient;  
        private IMemoryCache _memoryCache;  
        private IConfiguration _config;  
        private IRequestRepository _requestRepository;  
        private IResponseRepository _responseRepository;  
        private Result _responseResult;
        private CurrencyExchange _currencyExchangeRates;
        private string _currency;

        [SetUp]
        public void Setup()
        {
            _responseRepository = Substitute.For<IResponseRepository>();
            _requestRepository = Substitute.For<IRequestRepository>();
            _currencyExchangeClient = Substitute.For<ICurrencyExchangeClient>();
            _config = Substitute.For<IConfiguration>();
            _memoryCache = Create.MockedMemoryCache();
            _currency = "BTC";
            _responseResult = new Result
            {
                Data = new ResponseData
                {
                    Currency = _currency,
                    Rates = new Dictionary<string, string>()
                    {
                        { "ZAR","2000"}
                    }
                },
                ResponseBody = "",
                Method = "GET",
                StatusCode = 200,
                Uri = "example.com"
            };
            _currencyExchangeRates = new CurrencyExchange();
        }

        [Test]
        public async Task GetCurrencyExchangeRates_GivenNullCachedData_ShouldSaveUserRequestAndResponse()
        {
            //----------------------Arrange--------------------------------
            var currencyExchangeService = CreateCurrencyExchangeService();
            
            _config["CacheDuration"].Returns("1");
            _memoryCache.TryGetValue("exchangeRate", out _currencyExchangeRates).Returns(false);
            _currencyExchangeClient.GetLatestCurrencyExchangeRates(_currency).Returns(_responseResult);

            //----------------------Act------------------------------------
            await currencyExchangeService.GetCurrencyExchangeRates(_currency);

            //----------------------Assert---------------------------------
            await _requestRepository.Received().AddRequest(Arg.Any<UserRequest>());
            await _responseRepository.Received().AddResponse(Arg.Any<Response>());
        }

        [Test]
        public async Task GetCurrencyExchangeRates_GivenNullCachedData_ShouldGetLatestCurrencyExchangeRates()
        {
            //----------------------Arrange--------------------------------
            var currencyExchangeService = CreateCurrencyExchangeService();

            _config["CacheDuration"].Returns("1");
            _memoryCache.TryGetValue("exchangeRate", out _currencyExchangeRates).Returns(false);
            _currencyExchangeClient.GetLatestCurrencyExchangeRates(_currency).Returns(_responseResult);

            //----------------------Act------------------------------------
            await currencyExchangeService.GetCurrencyExchangeRates(_currency);

            //----------------------Assert---------------------------------
            await _currencyExchangeClient.Received().GetLatestCurrencyExchangeRates(_currency);
        }

        [Test]
        public async Task GetCurrencyExchangeRates_GivenValidCachedData_ShouldNotSaveUserRequestAndResponse()
        {
            //----------------------Arrange--------------------------------
            var currencyExchangeService = CreateCurrencyExchangeService();

            _memoryCache.TryGetValue("exchangeRate", out _currencyExchangeRates).Returns(true);

            //----------------------Act------------------------------------
            await currencyExchangeService.GetCurrencyExchangeRates(_currency);

            //----------------------Assert---------------------------------
            await _responseRepository.DidNotReceive().AddResponse(Arg.Any<Response>());
            await _requestRepository.DidNotReceive().AddRequest(Arg.Any<UserRequest>());
        }

        [Test]
        public async Task GetCurrencyExchangeRates_GivenValidCachedData_ShouldNotGetLatestCurrencyExchangeRates()
        {
            //----------------------Arrange--------------------------------
            var currencyExchangeService = CreateCurrencyExchangeService();

            _memoryCache.TryGetValue("exchangeRate", out _currencyExchangeRates).Returns(true);

            //----------------------Act------------------------------------
            await currencyExchangeService.GetCurrencyExchangeRates(_currency);

            //----------------------Assert---------------------------------
            await _currencyExchangeClient.DidNotReceive().GetLatestCurrencyExchangeRates(_currency);
        }

        [Test]
        public async Task GetCurrencyExchangeRates_GivenNullResultFromExchangeClient_ShouldNotSaveUserRequestAndResponse()
        {
            //----------------------Arrange--------------------------------
            var currencyExchangeService = CreateCurrencyExchangeService();

            _config["CacheDuration"].Returns("1");
            _memoryCache.TryGetValue("exchangeRate", out _currencyExchangeRates).Returns(false);
            await _currencyExchangeClient.GetLatestCurrencyExchangeRates(_currency);

            //----------------------Act------------------------------------
            await currencyExchangeService.GetCurrencyExchangeRates(_currency);

            //----------------------Assert---------------------------------
            await _responseRepository.DidNotReceive().AddResponse(Arg.Any<Response>());
            await _requestRepository.DidNotReceive().AddRequest(Arg.Any<UserRequest>());
        }

        private CurrencyExchangeService CreateCurrencyExchangeService()
        {
            return new CurrencyExchangeService(_currencyExchangeClient, _responseRepository, _requestRepository, _memoryCache, _config);
        }
    }
}
