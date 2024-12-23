using Abp.Authorization;
using Icon.Authorization.Roles;
using Icon.Authorization.Users;

namespace Icon.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
