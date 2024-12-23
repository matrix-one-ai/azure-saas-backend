using Microsoft.Extensions.Configuration;

namespace Icon.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
