using Marten;

namespace PPMRm
{
    public class PPMRmMartenRegistry : MartenRegistry
    {
        public PPMRmMartenRegistry()
        {
            //TODO: Define Aggregate & Entity configurations
            //For<Order>().Duplicate(x => x.OrderNumber);
        }
    }
}
