using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.ARTMIS.Orders
{
    public class OrderLine
    {
        public int LineNumber { get; set; }
        public int ROPrimeLineNumber { get; set; }
        public string ProductId { get; set; }
        public string ItemId { get; set; }
        public string UOM { get; set; }
        public decimal OrderedQuantity { get; set; }
    }
}
