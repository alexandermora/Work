using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServiceCoreV2.Models.Payments;

namespace WebServiceCoreV2.Models
{
    public class ShowDto<T>
    {
        public string Name { get; set; }
        public List<T> ListDto { get; set; }
    }
}
