using System;
using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace PPMRm
{
    public static class IdentityUserExtensions
    {
        private const string CountryIdPropertyName = "CountryId";

        public static void SetTitle(this IdentityUser user, string title)
        {
            user.SetProperty(CountryIdPropertyName, title);
        }

        public static string GetTitle(this IdentityUser user)
        {
            return user.GetProperty<string>(CountryIdPropertyName);
        }
    }
}
