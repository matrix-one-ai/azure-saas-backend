using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Icon.Authorization;

namespace Icon
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(IconApplicationSharedModule),
        typeof(IconCoreModule)
        )]
    public class IconApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IconApplicationModule).GetAssembly());
        }
    }
}