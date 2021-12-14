using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using System;

namespace PPMRm.Core
{
    public class CoreDataSeedContributor : IDataSeedContributor
    {
        private IRepository<Product, string> ProductRepository { get; }
        private IRepository<Country, string> CountryRepository { get; }
        private IRepository<Program, string> ProgramRepository { get; }


        public CoreDataSeedContributor(IRepository<Product, string> productRepository, IRepository<Country, string> countryRepository, IRepository<Program, string> programRepository)
        {
            ProductRepository = productRepository;
            CountryRepository = countryRepository;
            ProgramRepository = programRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if(await ProductRepository.GetCountAsync() <= 0)
            {

            }

            if(await CountryRepository.GetCountAsync() <= 0)
            {

            }

            if(await ProgramRepository.GetCountAsync() <= 0)
            {

            }
        }
    }


}
