using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceCoreV2.Models.Payments
{
    public class MethodPay
    {
        public string TypePayment { get; set; }
        public Payment PaymentValue { get; set; }
    }
}
