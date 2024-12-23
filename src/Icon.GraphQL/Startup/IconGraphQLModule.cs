using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Icon.Startup
{
    [DependsOn(typeof(IconCoreModule))]
    public class IconGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IconGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}