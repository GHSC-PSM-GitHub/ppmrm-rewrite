using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.ARTMIS.PeriodShipments
{
    public interface IPeriodShipmentRepository
    {
        Task<PeriodShipment> GetAsync(string countryId, int periodId);

        //Task<PeriodShipment> StoreAsync(PeriodShipment periodShipment);

    }
}
