using Abp.AspNetCore.Mvc.Views;

namespace Icon.Web.Views
{
    public abstract class IconRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected IconRazorPage()
        {
            LocalizationSourceName = IconConsts.LocalizationSourceName;
        }
    }
}
