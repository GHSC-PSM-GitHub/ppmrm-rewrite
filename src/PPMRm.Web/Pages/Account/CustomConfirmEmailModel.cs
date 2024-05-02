using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Identity;
using Volo.Abp.TextTemplating;

namespace PPMRm.Web.Pages.Account
{
    [AllowAnonymous]
    public class CustomConfirmEmailModel : PageModel
    {
        private readonly IdentityUserManager _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ITemplateRenderer _templateRenderer;
        public CustomConfirmEmailModel(IdentityUserManager userManager, IEmailSender emailSender, ITemplateRenderer templateRenderer)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _templateRenderer = templateRenderer;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId.IsNullOrWhiteSpace() || code.IsNullOrWhiteSpace()) return RedirectToPage("/Index");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound($"Unable to load user with ID '{userId}'.");

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if(result.Succeeded)
            {
                var userName = user.UserName;
                var email = user.Email;

                var body = await _templateRenderer.RenderAsync(
                    StandardEmailTemplates.Message,
                    new
                    {
                        message = $"New user {userName} with email address {email} has registered. Please approve/deny access in PPMRm Dashboard."
                    });
                try
                {
                    await _emailSender.SendAsync(
                        "eyassug@gmail.com,jraji@ghsc-psm.org,swang@ghsc-psm.org,jthomas@chemonics.com,jinsthomas@gmail.com,clemke@usaid.gov,nprintz@usaid.gov,chershey@usaid.gov,aball@usaid.gov,nhuhn@usaid.gov",
                        "New user registration",
                        body
                    );
                }
                catch (Exception)
                {

                }
                return Page();
            }

            return NotFound($"Unable to confirm Email address for user.");
        }
    }
}