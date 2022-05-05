using PPMRm.Core;
using PPMRm.PeriodReports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.Reports
{
    public interface IReportAppService
    {
        Task<List<StockStatusDto>> GetStockStatusAsync(GetStockStatusDto request);
        /// <summary>
        /// Gets a summary of stockouts, shortages, oversupply and cs updates for a period
        /// </summary>
        /// <param name="id">Integer Period ID (yyyyMM)</param>
        /// <returns></returns>
        Task<PeriodSummaryDto> GetAsync(int id);
    }

    public class PeriodSummaryDto
    {
        public PeriodDto Period { get; set; }
        public ProductCountryDto Stockouts
        {
            get
            {
                var stockouts = AllProducts.Select(x => new
                {
                    Product = x,
                    NumberOfCountries = CountrySummaries.Where(s => s.Stockouts.Any(p => p.Product.Id == x.Id)).Count()
                }).Where(x => x.NumberOfCountries > 0).OrderBy(x => x.Product.Name.Length);
                return new ProductCountryDto
                {
                    Labels = stockouts.Select(x => x.Product.Name).ToList(),
                    Data = stockouts.Select(x => x.NumberOfCountries).ToList()
                };
            }
        }

        public ProductCountryDto Shortages
        {
            get
            {
                var shortages = AllProducts.Select(x => new
                {
                    Product = x,
                    NumberOfCountries = CountrySummaries.Where(s => s.Shortages.Any(p => p.Product.Id == x.Id)).Count()
                }).Where(x => x.NumberOfCountries > 0).OrderBy(x => x.Product.Name.Length);
                return new ProductCountryDto
                {
                    Labels = shortages.Select(x => x.Product.Name).ToList(),
                    Data = shortages.Select(x => x.NumberOfCountries).ToList()
                };
            }
        }

        public ProductCountryDto Oversupplies
        {
            get
            {
                var oversupplies = AllProducts.Select(x => new
                {
                    Product = x,
                    NumberOfCountries = CountrySummaries.Where(s => s.Oversupply.Any(p => p.Product.Id == x.Id)).Count()
                }).Where(x => x.NumberOfCountries > 0).OrderBy(x => x.Product.Name.Length);
                return new ProductCountryDto
                {
                    Labels = oversupplies.Select(x => x.Product.Name).ToList(),
                    Data = oversupplies.Select(x => x.NumberOfCountries).ToList()
                };
            }
        }

        public List<CountrySummaryDto> CountrySummaries { get; set; } = new();
        public List<Products.ProductDto> AllProducts => CountrySummaries.SelectMany(x => x.Products)
                        .Where(x => x.MOSStatus != null && x.MOSStatus != MOSStatus.MinToMax)
                        .Select(x => new { x.Product.Id, x.Product.Name }).Distinct()
                        .Select(x => new Products.ProductDto { Id = x.Id, Name = x.Name }).ToList();

    }

    public class PeriodSummarySectionDto
    {
        
    }

    public class CountrySummaryDto
    {
        public CountryDto Country { get; set; }
        public CommoditySecurityUpdatesDto CSUpdates { get; set; }
        public List<ProgramProductDto> Products { get; set; }
        public List<ProgramProductDto> Stockouts => Products?.Where(x => x.SOH == 0).ToList();
        public List<ProgramProductDto> Shortages => Products?.Where(x => x.MOSStatus == MOSStatus.BelowMin).ToList();
        public List<ProgramProductDto> Oversupply => Products?.Where(x => x.MOSStatus == MOSStatus.OverStocked).ToList();
    }

    public class ProductCountryDto
    {
        public List<string> Labels { get; set; }
        public List<int> Data { get; set; }
    }
}
