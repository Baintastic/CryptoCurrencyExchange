using CryptoCurrencyExchange.Data.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace CryptoCurrencyExchange.Data.ExternalServices
{
    public class CurrencyExchangeClient : ICurrencyExchangeClient
    {
        private readonly IConfiguration Configuration;
        private readonly HttpClient _client;

        public CurrencyExchangeClient(IConfiguration configuration, HttpClient client)
        {
            Configuration = configuration;
            _client = client;
        }

        public async Task<Result?> GetLatestCurrencyExchangeRates(string currency)
        {
            try
            {
                var response = await _client.GetAsync($"{Configuration["CoinbaseAPIExchangeRateUrl"]}{currency}");
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Result? result = null;
                if (response.IsSuccessStatusCode)
                {
                    var deserializedResult = await response.Content.ReadFromJsonAsync<APIResult>(options);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    result = deserializedResult != null ? GetResult(response, deserializedResult, responseBody) : null;
                }
                return result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
        }

        private static Result GetResult(HttpResponseMessage response, APIResult? deserializedResult, string responseBody)
        {
            return new Result
            {
                Data = deserializedResult?.Data,
                StatusCode = (int)response.StatusCode,
                ResponseBody = responseBody,
                Uri = response.RequestMessage?.RequestUri?.ToString(),
                Method = response.RequestMessage?.Method.ToString()
            };
        }
    }
}
