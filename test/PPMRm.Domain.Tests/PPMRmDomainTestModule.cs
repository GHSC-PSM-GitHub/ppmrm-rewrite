using PPMRm.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace PPMRm
{
    [DependsOn(
        typeof(PPMRmEntityFrameworkCoreTestModule)
        )]
    public class PPMRmDomainTestModule : AbpModule
    {

    }
}