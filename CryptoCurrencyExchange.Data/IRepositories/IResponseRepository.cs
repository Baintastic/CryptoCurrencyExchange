using CryptoCurrencyExchange.Data.Models.Entities;

namespace CryptoCurrencyExchange.Data.IRepositories
{
    public interface IResponseRepository
    {
        Task AddResponse(Response response);
    }
}
