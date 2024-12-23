using Abp.Dependency;
using GraphQL.Types;
using Icon.Queries.Container;
using System;
using GraphQL.Conversion;
using Microsoft.Extensions.DependencyInjection;

namespace Icon.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
            NameConverter = new CamelCaseNameConverter();
        }
    }
}