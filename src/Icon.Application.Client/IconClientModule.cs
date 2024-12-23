using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Icon
{
    public class IconClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IconClientModule).GetAssembly());
        }
    }
}
