using Volo.Abp.Modularity;

namespace PPMRm
{
    [DependsOn(
        typeof(PPMRmApplicationModule),
        typeof(PPMRmDomainTestModule)
        )]
    public class PPMRmApplicationTestModule : AbpModule
    {

    }
}