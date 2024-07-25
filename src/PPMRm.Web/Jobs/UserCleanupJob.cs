using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Identity;
using Volo.Abp.TextTemplating;

namespace PPMRm.Web.Jobs
{
	public interface IUserCleanupJob
	{
        Task SendWarningEmailsToInactiveUsers();
        Task RemoveInactiveUsers();
    }


	public class UserCleanupJob : IUserCleanupJob
    {
        private readonly IIdentityUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly ITemplateRenderer _templateRenderer;
        public UserCleanupJob(ITemplateRenderer templateRenderer,
            IIdentityUserRepository userRepository,
            IEmailSender emailSender)
        {
            _templateRenderer = templateRenderer;
            _userRepository = userRepository;
            _emailSender = emailSender;
        }

        public ITemplateRenderer TemplateRenderer { get; }

        public async Task RemoveInactiveUsers()
        {

            var inactivityTimeSpan = 180;
            var users = await _userRepository.GetListAsync();
            var deletedUsers = users.Where(u => u.DaysSinceLastLogin() > inactivityTimeSpan);
            await _userRepository.DeleteManyAsync(deletedUsers, autoSave: true);
        }

        public async Task SendWarningEmailsToInactiveUsers()
        {
            var inactivityTimeSpan = 165;
            var users = await _userRepository.GetListAsync();
            var inactiveUsers = users.Where(u => u.EmailConfirmed && u.DaysSinceLastLogin() == inactivityTimeSpan).ToList();
            foreach (var user in inactiveUsers)
            {
                var lastLogin = user.DaysSinceLastLogin();
                var body = await _templateRenderer.RenderAsync(
                    StandardEmailTemplates.Message,
                    new
                    {
                        message = $"Your PPMRm user account with this email address is expiring in 15 days. Please login to the PPMRm platform to keep your account."
                    });
                await _emailSender.SendAsync(
                    user.Email,
                    "Account Deactivation",
                    body
                );
            }
        }
    }
}

