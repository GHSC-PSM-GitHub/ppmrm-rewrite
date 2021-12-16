using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace PPMRm.Core
{
    public class CoreDataSeedContributor : IDataSeedContributor, ITransientDependency
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

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            if(await ProductRepository.GetCountAsync() <= 0)
            {

            }

            if(await CountryRepository.GetCountAsync() <= 0)
            {
                var countries = new List<Country>
                {
                    new Country("Angola"){ Name = "Angola"},
                    new Country("Benin"){ Name = "Benin"},
                    new Country("Burkina Faso"){ Name = "Burkina Faso"},
                    new Country("Burundi"){ Name = "Burundi"},
                    new Country("Cambodia"){ Name = "Cambodia"},
                    new Country("Cameroon"){ Name = "Cameroon"},
                    new Country("Côte d'Ivoire"){ Name = "Côte d'Ivoire"},
                    new Country("Eswatini"){ Name = "Eswatini"},
                    new Country("Ethiopia"){ Name = "Ethiopia"},
                    new Country("Ghana"){ Name = "Ghana"},
                    new Country("Guinea"){ Name = "Guinea"},
                    new Country("Kenya"){ Name = "Kenya"},
                    new Country("Laos"){ Name = "Laos"},
                    new Country("Liberia"){ Name = "Liberia"},
                    new Country("Madagascar"){ Name = "Madagascar"},
                    new Country("Malawi"){ Name = "Malawi"},
                    new Country("Mali"){ Name = "Mali"},
                    new Country("Mozambique"){ Name = "Mozambique"},
                    new Country("Myanmar"){ Name = "Myanmar"},
                    new Country("Niger"){ Name = "Niger"},
                    new Country("Nigeria"){ Name = "Nigeria"},
                    new Country("Rwanda"){ Name = "Rwanda"},
                    new Country("Senegal"){ Name = "Senegal"},
                    new Country("Sierra Leone"){ Name = "Sierra Leone"},
                    new Country("South Sudan"){ Name = "South Sudan"},
                    new Country("Tanzania"){ Name = "Tanzania"},
                    new Country("Thailand"){ Name = "Thailand"},
                    new Country("Uganda"){ Name = "Uganda"},
                    new Country("United States"){ Name = "United States"},
                    new Country("Zambia"){ Name = "Zambia"},
                    new Country("Zimbabwe"){ Name = "Zimbabwe"}
                };
                await CountryRepository.InsertManyAsync(countries);
            }

            if(await ProgramRepository.GetCountAsync() <= 0)
            {

            }
        }
    }

}
