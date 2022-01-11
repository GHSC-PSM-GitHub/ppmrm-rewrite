using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace PPMRm.Core
{
    public class CountryAppService : CrudAppService<
            Country,
            CountryDto,
            string, //Primary key 
            PagedAndSortedResultRequestDto,
            CountryDto>,
        ICountryAppService
    {
        IRepository<IdentityUser, Guid> UserRepository { get; }
        public CountryAppService(IRepository<Country, string> repository, IRepository<IdentityUser, Guid> userRepository) : base(repository)
        {
            UserRepository = userRepository;
        }

        [Authorize]
        async public Task<PagedResultDto<CountryDto>> GetUserCountryListAsync(PagedAndSortedResultRequestDto input)
        {
            var user = await UserRepository.GetAsync(CurrentUser.Id.Value);
            if (user.GetUserType() == Identity.UserType.DataProvider)
            {
                var userCountry = await Repository.GetAsync(user.GetCountryId());
                var list = new List<CountryDto>();
                if (userCountry != null) list.Add(ObjectMapper.Map<Country, CountryDto>(userCountry));
                return new PagedResultDto<CountryDto>(list.Count, list);
            }
            return await GetListAsync(input);
        }
    }
}
