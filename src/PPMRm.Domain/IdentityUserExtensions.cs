using System;
using PPMRm.Identity;
using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace PPMRm
{
    public static class IdentityUserExtensions
    {
        private const string CountryIdPropertyName = IdentityConsts.UserExtensionProperties.CountryId;

        public static void SetCountryId(this IdentityUser user, string countryId)
        {
            user.SetProperty(CountryIdPropertyName, countryId);
        }

        public static string GetCountryId(this IdentityUser user)
        {
            return user.GetProperty<string>(CountryIdPropertyName);
        }

        public static UserType GetUserType(this IdentityUser user)
        {
            return user.GetProperty(IdentityConsts.UserExtensionProperties.UserType, UserType.DataProvider);
        }

        public static void SetUserType(this IdentityUser user, UserType value)
        {
            user.SetProperty(IdentityConsts.UserExtensionProperties.UserType, value);
        }
    }
}
