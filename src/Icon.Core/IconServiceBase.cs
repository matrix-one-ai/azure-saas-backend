using Abp;

namespace Icon
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="IconDomainServiceBase"/>.
    /// For application services inherit IconAppServiceBase.
    /// </summary>
    public abstract class IconServiceBase : AbpServiceBase
    {
        protected IconServiceBase()
        {
            LocalizationSourceName = IconConsts.LocalizationSourceName;
        }
    }
}