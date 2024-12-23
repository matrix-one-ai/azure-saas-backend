using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Icon.Authorization.Users;
using Icon.MultiTenancy;

namespace Icon.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}