using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Icon.Configure;
using Icon.Startup;
using Icon.Test.Base;

namespace Icon.GraphQL.Tests
{
    [DependsOn(
        typeof(IconGraphQLModule),
        typeof(IconTestBaseModule))]
    public class IconGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IconGraphQLTestModule).GetAssembly());
        }
    }
}