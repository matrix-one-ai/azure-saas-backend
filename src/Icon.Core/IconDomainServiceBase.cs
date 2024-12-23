using Abp.Domain.Services;

namespace Icon
{
    public abstract class IconDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected IconDomainServiceBase()
        {
            LocalizationSourceName = IconConsts.LocalizationSourceName;
        }
    }
}
