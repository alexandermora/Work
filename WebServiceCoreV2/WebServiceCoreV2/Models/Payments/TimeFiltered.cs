using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceCoreV2.Models.Payments
{
    public class TimeFiltered
    {
        public string Time { get; set; }
        public PaymentFiltered Value { get; set; }
    }
}
