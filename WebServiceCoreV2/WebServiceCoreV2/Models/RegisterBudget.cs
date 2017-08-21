using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceCoreV2.Models
{
    public class RegisterBudget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TypeCatalogue { get; set; }
        public double ValuePay { get; set; }
        public string Currency { get; set; }
        public string DateBuy { get; set; }
        public string TimeZone { get; set; }
        public string Support { get; set; }
        public int IdUser { get; set; }
    }
}
