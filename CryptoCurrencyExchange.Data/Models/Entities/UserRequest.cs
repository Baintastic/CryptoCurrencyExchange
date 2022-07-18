using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyExchange.Data.Models.Entities
{
    public class UserRequest
    {
        public int Id { get; set; }
        public string? Header { get; set; }
        public string? RequestMethod { get; set; }
        public string? Body { get; set; }
        public string? Url { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
