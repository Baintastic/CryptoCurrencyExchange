using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyExchange.Data.Models.Entities
{
    public class Response
    {
        public int Id { get; set; }
        public string? Header { get; set; }
        public int? StatusCode { get; set; }
        public string? Body { get; set; }
        public DateTime ResponseDate { get; set; }

        public int UserRequestId { get; set; }
        public virtual UserRequest? UserRequest { get; set; }
    }
}
