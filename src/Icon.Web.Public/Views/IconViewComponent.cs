using Abp.AspNetCore.Mvc.ViewComponents;

namespace Icon.Web.Public.Views
{
    public abstract class IconViewComponent : AbpViewComponent
    {
        protected IconViewComponent()
        {
            LocalizationSourceName = IconConsts.LocalizationSourceName;
        }
    }
}