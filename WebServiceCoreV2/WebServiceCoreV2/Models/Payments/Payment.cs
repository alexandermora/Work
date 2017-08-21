using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceCoreV2.Models.Payments
{
    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TypeCatalogue { get; set; }
        public double ValuePay { get; set; }
        public string Currency { get; set; }
        public string DatePay { get; set; }
        public string Timezone { get; set; }
        public string Support { get; set; }
    }
}
