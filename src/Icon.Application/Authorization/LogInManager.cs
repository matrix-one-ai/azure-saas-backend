using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Icon.Authorization.Roles;
using Icon.Authorization.Users;
using Icon.MultiTenancy;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Icon.Authorization
{
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        public LogInManager(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher,
            UserClaimsPrincipalFactory claimsPrincipalFactory)
            : base(
                  userManager,
                  multiTenancyConfig,
                  tenantRepository,
                  unitOfWorkManager,
                  settingManager,
                  userLoginAttemptRepository,
                  userManagementConfig,
                  iocResolver,
                  passwordHasher,
                  roleManager,
                  claimsPrincipalFactory)
        {

        }

        /// <summary>
        /// Exposes protected method CreateLoginResultAsync
        /// </summary>
        /// <param name="user">User to create login result</param>
        /// <param name="tenant">Tenant of the given user</param>
        /// <returns></returns>
        public Task<AbpLoginResult<Tenant, User>> CreateLoginResultAsync(User user, Tenant tenant = null)

        {
            return base.CreateLoginResultAsync(user, tenant);
        }
    }
}