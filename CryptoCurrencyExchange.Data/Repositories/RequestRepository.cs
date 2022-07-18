using CryptoCurrencyExchange.Data.IRepositories;
using CryptoCurrencyExchange.Data.Models;
using CryptoCurrencyExchange.Data.Models.Entities;

namespace CryptoCurrencyExchange.Data.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ExchangeRateDbContext _context;

        public RequestRepository(ExchangeRateDbContext context)
        {
            _context = context;
        }

        public async Task AddRequest(UserRequest request)
        {
            _context?.UserRequests?.AddAsync(request);
            await _context?.SaveChangesAsync();
        }
    }
}
