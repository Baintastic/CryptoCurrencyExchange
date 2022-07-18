using CryptoCurrencyExchange.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CryptoCurrencyExchange.Data.Models
{
    public class ExchangeRateDbContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public ExchangeRateDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<UserRequest>? UserRequests { get; set; }
        public DbSet<Response>? Responses { get; set; }
    }
}
