using System;
using System.Collections.Generic;
using System.Text;

namespace PPMRm.PeriodReports
{
    [Flags]
    public enum SOHLevel
    {
        CentralWarehouse = 1,
        ZonalWarehouse = 2,
        RegionalWarehouse= 4,
        ProvincialWarehouse=8,
        StateWarehouse=16,
        LGAWarehouse=32,
        IntermediateWarehouse=64,
        DistrictWarehouse=128,
        CountyWarehouse=256,
        MunicipalWarehouse=512,
        SubCountryWarehouse=1024,
        SubDistrictWarehouse=2048,
        ServiceDeliveryPoints=4096,
        //JointMedicalStore=
    }
}
