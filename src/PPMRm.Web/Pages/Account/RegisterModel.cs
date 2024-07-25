using System;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Data;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;

namespace PPMRm.Web.Pages.Account
{
    public class CustomRegisterModel : RegisterModel
    {
        private readonly IEmailSender _emailSender;
        IDataFilter DataFilter { get; }
        IEmailSender EmailSender { get; }
        ITemplateRenderer TemplateRenderer { get; }

        public CustomRegisterModel(
            IAccountAppService accountAppService,
            IEmailSender emailSender,
            ITemplateRenderer templateRenderer,
            IDataFilter dataFilter) : base(accountAppService)
        {
            _emailSender = emailSender;
            DataFilter = dataFilter;
        }

        public override async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await RegisterLocalUserAsync();
                return RedirectToPage("RegisterConfirmation", new { email = Input.EmailAddress, returnUrl = ReturnUrl });
            }
            catch (BusinessException e)
            {
                Alerts.Danger(e.Message);
                return Page();
            }
        }

        protected override async Task RegisterLocalUserAsync()
        {
            ValidateModel();
            using(DataFilter.Disable<ISoftDelete>())
            {
                var existingUser = await UserManager.FindByEmailAsync(Input.EmailAddress);
                if(existingUser != null && existingUser.IsDeleted)
                {
                    existingUser.IsDeleted = false;
                    existingUser.SetEmailConfirmed(false);
                    existingUser.SetUserType(Identity.UserType.Draft);
                    await UserManager.SetUserNameAsync(existingUser, Input.UserName);
                    await UserManager.RemovePasswordAsync(existingUser);
                    await UserManager.AddPasswordAsync(existingUser, Input.Password);
                    await UserManager.UpdateAsync(existingUser);
                    await SendEmailToAskForEmailConfirmationAsync(existingUser);
                    return;

                }
            }

            var userDto = await AccountAppService.RegisterAsync(
                new RegisterDto
                {
                    AppName = "MVC",
                    EmailAddress = Input.EmailAddress,
                    Password = Input.Password,
                    UserName = Input.UserName
                }
            );

            var user = await UserManager.GetByIdAsync(userDto.Id);
            user.SetUserType(Identity.UserType.Draft);
            await UserManager.UpdateAsync(user);
            await SendEmailToAskForEmailConfirmationAsync(user);
        }

        private async Task SendRegistrationEmail(IdentityUser user)
        {
            var body = await TemplateRenderer.RenderAsync(
                StandardEmailTemplates.Message,
                new
                {
                    message = $"New user {user.UserName} with email address {user.Email} has registered. Please approve/deny access in PPMRm Dashboard."
                }
            );
            try
            {
                await _emailSender.SendAsync(
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
        
        private async Task SendEmailToAskForEmailConfirmationAsync(IdentityUser user)
        {
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page("/Account/ConfirmEmail", pageHandler: null, values: new { userId = user.Id, code = code }, protocol: Request.Scheme);

            // TODO use EmailService instead of using IEmailSender directly
            await _emailSender.SendAsync(Input.EmailAddress, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }
    }
}