using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.Reports
{
    public interface IReportAppService
    {
        Task<List<StockStatusDto>> GetStockStatusAsync(GetStockStatusDto request);
    }
}
