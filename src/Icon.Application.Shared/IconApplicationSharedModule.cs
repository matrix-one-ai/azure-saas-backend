using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Icon
{
    [DependsOn(typeof(IconCoreSharedModule))]
    public class IconApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IconApplicationSharedModule).GetAssembly());
        }
    }
}