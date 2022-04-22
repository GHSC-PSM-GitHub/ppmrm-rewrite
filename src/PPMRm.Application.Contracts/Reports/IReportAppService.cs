using System.Collections.Generic;
using System.Text;

namespace PPMRm.Reports
{
    public interface IReportAppService
    {
        List<StockStatusDto> GetStockStatusAsync(GetStockStatusDto request);
    }
}
