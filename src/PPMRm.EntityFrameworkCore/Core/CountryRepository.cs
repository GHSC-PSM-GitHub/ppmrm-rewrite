using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PPMRm.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace PPMRm.Core
{
    public class CountryRepository : EfCoreRepository<PPMRmDbContext, Country, string>, ICountryRepository
    {

        IRepository<IdentityUser, Guid> UserRepository { get; }
        IAuthorizationService AuthorizationService { get; }
        ICurrentUser CurrentUser { get; }
        public CountryRepository(IDbContextProvider<PPMRmDbContext> dbContextProvider, IAuthorizationService authorizationService, ICurrentUser currentUser, IRepository<IdentityUser, Guid> userRepository)
        : base(dbContextProvider)
        {
            AuthorizationService = authorizationService;
            UserRepository = userRepository;
            CurrentUser = currentUser;
        }
        async public Task<ICollection<Country>> GetUserCountriesAsync()
        {
            if (!CurrentUser.IsAuthenticated) throw new UnauthorizedAccessException();
            var result = await AuthorizationService.AuthorizeAsync(PPMRmConsts.Permissions.DataReviewer);

            var dbContext = await GetDbContextAsync();
            if (result.Succeeded) // Data Reviewer / Admin
                return await dbContext.Set<Country>().OrderBy(c => c.Name).ToListAsync();

            var user = await UserRepository.GetAsync(CurrentUser.Id.Value);
            if (user.GetUserType() != Identity.UserType.DataProvider)
                return await dbContext.Set<Country>().ToListAsync();

            return await dbContext.Set<Country>().Where(c => c.Id == user.GetCountryId()).ToListAsync();
        }
    }
}
