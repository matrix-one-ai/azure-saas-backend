using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Icon
{
    public class IconCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IconCoreSharedModule).GetAssembly());
        }
    }
}