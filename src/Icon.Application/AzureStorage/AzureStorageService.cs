using Microsoft.Extensions.Configuration;
using Icon.Matrix;
using Icon.Configuration;
using Abp.Authorization;

namespace Icon.AzureStorage
{

    [AbpAuthorize]
    public class AzureStorageService : IconAppServiceBase
    {
        private IConfigurationRoot _configuration;
        private IMemoryManager _memoryManager;
        private IPlatformManager _platformManager;
        private ICharacterManager _characterManager;
        public AzureStorageService(
            IAppConfigurationAccessor appConfigurationAccessor,
            IMemoryManager memoryManager,
            IPlatformManager platformManager,
            ICharacterManager characterManager)
        {
            _configuration = appConfigurationAccessor.Configuration;
            _memoryManager = memoryManager;
            _platformManager = platformManager;
            _characterManager = characterManager;
        }
    }
}