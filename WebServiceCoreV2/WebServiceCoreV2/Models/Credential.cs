using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceCoreV2.Models
{
    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Group { get; set; }
        public string Country { get; set; }
    }
}
