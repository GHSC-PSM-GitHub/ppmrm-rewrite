using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;
using Volo.Abp.Security.Encryption;
using Volo.Abp.Settings;
using Volo.Abp.TextTemplating;

namespace PPMRm.Web.Jobs
{
    public class UserRegistrationHandler : ILocalEventHandler<EntityCreatedEventData<IdentityUser>>,
          ITransientDependency
    {
        IEmailSender EmailSender { get; }
        ITemplateRenderer TemplateRenderer { get; }
        ILogger<UserRegistrationHandler> Logger { get; }
        public UserRegistrationHandler(IEmailSender emailSender, ITemplateRenderer templateRenderer, ILogger<UserRegistrationHandler> logger)
        {
            EmailSender = emailSender;
            TemplateRenderer = templateRenderer;
            Logger = logger;
        }
        public async Task HandleEventAsync(
            EntityCreatedEventData<IdentityUser> eventData)
        {
            var userName = eventData.Entity.UserName;
            var email = eventData.Entity.Email;
            
            var body = await TemplateRenderer.RenderAsync(
                StandardEmailTemplates.Message,
                new
                {
                    message = $"New user {userName} with email address {email} has registered. Please approve/deny access in PPMRm Dashboard."
                }
            );
            try
            {
                await EmailSender.SendAsync(
                    "eyassug@gmail.com,jraji@ghsc-psm.org,swang@ghsc-psm.org,jthomas@chemonics.com,jinsthomas@gmail.com,clemke@usaid.gov,nprintz@usaid.gov,chershey@usaid.gov,aball@usaid.gov,nhuhn@usaid.gov",
                    "New user registration",
                    body
                );
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not send outgoing email.");
            }
        }
    }
}