using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Icon.ApiClient;
using Icon.Maui.Core;

namespace Icon.Maui
{
    [DependsOn(typeof(IconClientModule), typeof(AbpAutoMapperModule))]
    public class IconMauiModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.ReplaceService<IApplicationContext, MauiApplicationContext>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IconMauiModule).GetAssembly());
        }
    }
}