namespace PPMRm.Repository
{
    using System;
    using Entities;
    using Volo.Abp.Domain.Repositories;

    public interface IOrderRepository : IRepository<Order, string>
    {
    }
}
