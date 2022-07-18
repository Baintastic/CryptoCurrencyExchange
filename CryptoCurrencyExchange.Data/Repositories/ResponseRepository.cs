using CryptoCurrencyExchange.Data.IRepositories;
using CryptoCurrencyExchange.Data.Models;
using CryptoCurrencyExchange.Data.Models.Entities;

namespace CryptoCurrencyExchange.Data.Repositories
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly ExchangeRateDbContext _context;

        public ResponseRepository(ExchangeRateDbContext context)
        {
            _context = context;
        }

        public async Task AddResponse(Response response)
        {
            _context?.Responses?.AddAsync(response);
            await _context?.SaveChangesAsync();
        }
    }
}
