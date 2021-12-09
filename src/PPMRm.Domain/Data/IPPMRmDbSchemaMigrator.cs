using System.Threading.Tasks;

namespace PPMRm.Data
{
    public interface IPPMRmDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
