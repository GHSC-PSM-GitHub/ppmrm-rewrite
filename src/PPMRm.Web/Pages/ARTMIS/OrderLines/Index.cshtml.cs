using Marten;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPMRm.Core;
using PPMRm.Items;
using PPMRm.Products;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using System;
using PPMRm.ARTMIS;

namespace PPMRm.Web.Pages.ARTMIS.OrderLines
{
    public class IndexModel : PageModel
    {
        IRepository<Product, string> ProductRepository { get; }
        IRepository<Country, string> CountryRepository { get; }
        IItemRepository ItemRepository { get; }
        IDocumentSession Session { get; }
        public IndexModel(IRepository<Country, string> countryRepository, IRepository<Product, string> productRepository, IItemRepository itemRepository, IDocumentSession session)
        {
            ProductRepository = productRepository;
            CountryRepository = countryRepository;
            ItemRepository = itemRepository;
            Session = session;
        }

        public async Task OnGetAsync()
        {
            Countries = (await CountryRepository.ToListAsync()).OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            Products = (await ProductRepository.ToListAsync()).OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            SelectedCountries = Countries.Select(c => c.Value).ToList();
            SelectedProducts = Products.Select(p => p.Value).ToList();
            var lastEvent = await Session.Events.QueryAllRawEvents().OrderByDescending(e => e.Sequence).FirstOrDefaultAsync();
            var timestamp = (lastEvent.Data as OrderLineEvent)?.FileName.Substring(0, ARTMISConsts.TimestampLength);
            SnapshotTimestamp = timestamp != null ? DateTime.ParseExact(timestamp, ARTMISConsts.DateTimeFormat, null) : null;
        }
        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Products { get; set; } = new();
        [DisplayName("Countries")]
        public List<string> SelectedCountries { get; set; } = new();
        [DisplayName("Products")]
        public List<string> SelectedProducts { get; set; } = new();

        public DateTime? SnapshotTimestamp { get; set; }
    }
}
