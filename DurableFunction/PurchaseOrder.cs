using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableFunction
{
    public class PurchaseOrder
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("customer")]
        public  string Customer { get; set; }
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
