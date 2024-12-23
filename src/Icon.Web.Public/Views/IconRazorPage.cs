using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Icon.Web.Public.Views
{
    public abstract class IconRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected IconRazorPage()
        {
            LocalizationSourceName = IconConsts.LocalizationSourceName;
        }
    }
}
