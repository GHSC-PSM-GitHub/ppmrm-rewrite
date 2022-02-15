using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PPMRm.Shipments
{
    public class ShipmentAppService : ApplicationService, IShipmentAppService
    {
        IDocumentStore DocumentStore { get; }
        public ShipmentAppService(IDocumentStore documentStore) => DocumentStore = documentStore;

        public async Task<PagedResultDto<ShipmentDto>> GetListAsync(PagedResultRequestDto input)
        {
            //using var session = DocumentStore.LightweightSession();
            //var products = await session.Query<Items.Item>().ToListAsync();
            //var productDtos = ObjectMapper.Map<List<Items.Item>, List<Items.ItemDto>>(products.ToList());
            //var orderLines = session.Query<ARTMIS.Orders.Order>().Where(o => o.DisplayDate > new DateTime(2021, 12, 1) && o.Lines.Any()).OrderBy(o => o.CountryId)
            //                    .SelectMany(o => o.Lines.Select(l => new { o, l });
            //var lineCount = await orderLines.CountAsync();
            //var orderCount = await session.Query<ARTMIS.Orders.Order>().Where(o => o.DisplayDate > new DateTime(2021, 12, 1) && o.Lines.Any()).OrderBy(o => o.CountryId).ToListAsync();
            //var totalCount = orderCount.Select(o => o.Lines.Count).Sum(c => c);
            //var orders = session.Query<ARTMIS.Orders.Order>().Where(o => o.DisplayDate > new DateTime(2021, 12, 1) && o.Lines.Any()).OrderBy(i => i.CountryId);
            //try
            //{
            //    var result = orderCount.SelectMany(o => o.Lines.Select(l => new { o, l })
            //                       .Select(x =>
            //                           new ShipmentDto
            //                           {
            //                               CountryId = x.o.CountryId,
            //                               Item = productDtos.SingleOrDefault(p => p.Id == x.l.ProductId),
            //                               LineNumber = x.l.LineNumber,
            //                               OrderedQuantity = x.l.OrderedQuantity,
            //                               OrderNumber = x.o.OrderNumber,
            //                               RONumber = x.o.RONumber,
            //                               ROPrimeLineNumber = x.l.ROPrimeLineNumber,
            //                               UOM = x.l.UOM
            //                           }
            //                       )).Skip(input.SkipCount).Take(input.MaxResultCount).ToList(); ;
            //    return new PagedResultDto<ShipmentDto>(totalCount, result);

            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
            throw new Exception();
            
        }
    }
}
