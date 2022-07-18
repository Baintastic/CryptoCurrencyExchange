using CryptoCurrencyExchange.Data.Models.Entities;

namespace CryptoCurrencyExchange.Data.IRepositories
{
    public interface IRequestRepository
    {
        Task AddRequest(UserRequest request);
    }
}
