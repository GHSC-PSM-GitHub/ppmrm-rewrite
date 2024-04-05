using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;

namespace PPMRm.Web.Jobs
{
    public class UserLastLoginUpdateHandler : ILocalEventHandler<EntityCreatedEventData<IdentitySecurityLog>>,
          ITransientDependency
    {
          
        ILogger<UserLastLoginUpdateHandler> Logger    { get; }

        private readonly IIdentityUserRepository _identityUserRepository;
        public UserLastLoginUpdateHandler(  ILogger<UserLastLoginUpdateHandler> logger, IIdentityUserRepository identityUserRepository)
        { 
            Logger = logger;
            _identityUserRepository = identityUserRepository;
        }
        public async Task HandleEventAsync(
            EntityCreatedEventData<IdentitySecurityLog> eventData)
        {
            Guid userid = eventData.Entity.UserId.Value;
            var action = eventData.Entity.Action;

            if (action == IdentitySecurityLogActionConsts.LoginSucceeded)
            {

                var users = await _identityUserRepository.GetAsync(userid);

                try
                {
                    IdentityUserExtensions.SetUserLastLogin(users, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString() );
                    await _identityUserRepository.UpdateAsync(users);
                }
                catch(Exception ex)
                {
                    Logger.LogError(ex, "Could not update User LastLogin");
                }
            }
        }
    }
}