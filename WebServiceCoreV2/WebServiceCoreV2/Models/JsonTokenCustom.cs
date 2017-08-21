using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceCoreV2.Models
{
    public class JsonTokenCustom
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }
        [JsonProperty("expires_in")]
        public double Expires { get; set; }
    }
}
