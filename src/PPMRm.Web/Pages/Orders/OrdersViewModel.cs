using PPMRm.Core;
using System.Collections.Generic;

namespace PPMRm.Web.Pages.Orders
{
    public class OrdersViewModel
    {
        public List<CountryDto> Countries { get; set; }
        public List<PeriodDto> Periods { get; set; }
    }
}
