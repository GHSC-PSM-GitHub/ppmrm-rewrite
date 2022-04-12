using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;

namespace PPMRm.ARTMIS.OrderLines
{
    public class OrderLineAppService : ApplicationService, IOrderLineAppService
    {
        IRepository<Core.Country, string> CountryRepository { get; }
        IRepository<Products.Product, string> ProductRepository { get; }
        IDocumentSession Session { get; }
        public OrderLineAppService(IDocumentSession session, IRepository<Core.Country, string> countryRepository, IRepository<Products.Product, string> productRepository)
        {
            Session = session;
            CountryRepository = countryRepository;
            ProductRepository = productRepository;
        }
        public Task<OrderLineDto> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        async public Task<PagedResultDto<OrderLineDto>> GetListAsync(GetOrderLinesDto input)
        {
            Expression<Func<OrderLine, bool>> filter = l => input.Countries.Contains(l.CountryId) && input.Products.Contains(l.PPMRmProductId) && 
                                                (l.ActualDeliveryDate >= input.ShipmentDateAfter || l.ActualDeliveryDate == null);
            var queryable = Session.Query<OrderLine>().Where(filter);
            //var countriesQueryable = await CountryRepository.GetQueryableAsync();
            //var productsQueryable = await ProductRepository.GetQueryableAsync();
            var countries = CountryRepository.Where(c => input.Countries.Contains(c.Id)).ToList();
            var products = ProductRepository.Where(p => input.Products.Contains(p.Id)).ToList();
            var items = await Session.Query<Items.Item>().ToListAsync();
            var totalCount = await queryable.CountAsync();
            var results = (from l in await queryable.OrderBy(q => q.CountryId).Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync()
                           join c in countries on l.CountryId equals c.Id
                           join p in products on l.PPMRmProductId equals p.Id
                           join i in items on l.ProductId equals i.Id
                           orderby c.Name
                           select new
                           {
                               OrderLine = l,
                               Country = c,
                               Product = p,
                               Item = i
                           }).ToList();
            var result = results.Select(r => new
            {
                OrderLine = ObjectMapper.Map<OrderLine, OrderLineDto>(r.OrderLine),
                Country = r.Country,
                Product = r.Product,
                Item = r.Item
            }).ToList();
            
            result.ForEach(r =>
            {
                r.OrderLine.Country = new Core.CountryDto { Id = r.Country.Id, Name = r.Country.Name };
                r.OrderLine.Product = new Products.ProductDto { Id = r.Product.Id, Name = r.Product.Name };
                r.OrderLine.Item = new Items.ItemDto { Id = r.Item.Id, Name = r.Item.Name, BaseUnitMultiplier = r.Item.BaseUnitMultiplier };
            });

            var response = result.Select(r => r.OrderLine).ToList();
                
            //var response = result.Select(r => new
            //{
            //    OrderLine = ObjectMapper.Map<OrderLine, OrderLineDto>(r.OrderLine),
            //    Country = ObjectMapper.Map<Core.Country, Core.CountryDto>(r.Country),
            //    Product = ObjectMapper.Map<Products.Product, Products.ProductDto>(r.Product)
            //}).ToList();
            //response.ForEach(r =>
            //{
            //});
            return new PagedResultDto<OrderLineDto>(totalCount, response);
            //var response = result.Select(r => ObjectMapper.Map<OrderLine, OrderLineDto>(r.OrderLine)).ToList().ForEach(l =>
            //{
            //    l.Country = ObjectMapper.Map<Core.Country, Core.CountryDto>(result.Single(r => r.Country.Id == l.CountryId).Country);
            //    l.Product = ObjectMapper.Map<Products.Product, Core.Coun>
            //});
            //var countries = await CountryRepository.Where(c => results.Select(r => r.CountryId).Contains(c.Id)).ToListAsync();
            //var products = await ProductRepository.Where(p => results.Select(r => r.ProductId).Contains(p.Id)).ToListAsync();
            //var response = results.Select(r => ObjectMapper.Map<OrderLine, OrderLineDto>(r)).ToList();
            //foreach (var item in response)
            //{
            //    item.Country = ObjectMapper.Map<Core.Country, Core.CountryDto>(countries.Single(c => c.Id == item.CountryId));
            //    item.Product = ObjectMapper.Map<Products.Product, Products.ProductDto>(products.Single(c => c.Id == item.ProductId));
            //}
            //return new PagedResultDto<OrderLineDto>(totalCount, response);

        }
    }
}
